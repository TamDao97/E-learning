using Elearning.Model.Entities;
using Elearning.Model.Models.User.User;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System.Net.Http;
using Newtonsoft.Json;
using System.Net;
using System.IO;
using Elearning.Models.Combobox;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System.Runtime.Caching;
using Microsoft.Extensions.Caching.Memory;
using Wangkanai.Detection.Services;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using NTS.Common.Utils;
using System.Net.Mail;
using Elearning.Model.Models.UserAdmins;
using System.Text.RegularExpressions;
using Elearning.Model.Models.Mobile.Learner;
using Elearning.Model.Models.Fontend.User;
using Elearning.Models.Settings;
using Microsoft.Extensions.Options;
using Elearning.Model.Models.Mobile.User;
using Elearning.Model.Models.User.Learner;

namespace Elearning.Services.Fontend.User
{
    public class LoginService : ILoginService
    {

        private readonly ElearningContext sqlContext;
        private readonly IDetectionService _detection;
        private readonly IPasswordUtilsService passwordUtilsService;
        private readonly AppSettingModel appSettingModel;


        private IMemoryCache _cache;

        public LoginService(ElearningContext sqlContext, IMemoryCache _cache, IDetectionService _detection, IPasswordUtilsService passwordUtilsService, IOptions<AppSettingModel> options)
        {
            this.sqlContext = sqlContext;
            this._cache = _cache;
            this._detection = _detection;
            this.passwordUtilsService = passwordUtilsService;
            this.appSettingModel = options.Value;

        }
        public async Task CreateUserAsync(UserLearnerCreateModel model)
        {

            var data = sqlContext.Learner.Where(x => x.Email.Trim().ToUpper().Equals(model.Email.Trim().ToUpper()) && x.Provider.Equals("Email")).FirstOrDefault();
            string securityStamp = passwordUtilsService.CreatePasswordHash();
            string passwordHash = passwordUtilsService.ComputeHash(model.Password + securityStamp);

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(model.Password) && hasUpperChar.IsMatch(model.Password) && hasLowerChar.IsMatch(model.Password) && hasSymbols.IsMatch(model.Password);

            if (isValidated == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0045);
            }

            if (string.IsNullOrEmpty(model.Name))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018, TextResourceKey.Name);
            }

            if (string.IsNullOrEmpty(model.Email))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018, TextResourceKey.Email);
            }

            if (data != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0043, TextResourceKey.User);
            }

            Learner learner = new Learner()
            {
                Id = Guid.NewGuid().ToString(),
                Name = model.Name.Trim(),
                Email = model.Email.Trim(),
                SecurityStamp = securityStamp,
                PasswordHash = passwordHash,
                CreateDate = DateTime.Now,
                IsLogin = false,
                Provider = "Email",
                UpdateDate = DateTime.Now,
            };

            sqlContext.Learner.Add(learner);
            await sqlContext.SaveChangesAsync();

            //await SendEmail(learner.Email, model.PasswordHash);

        }

        private async Task SendEmail(string email, string passwordHash)
        {
            var list = await sqlContext.SystemParams.ToListAsync();

            string fromaddr = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP01.ToUpper()))?.ParamValue;
            string toaddr = email;
            string password = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP02.ToUpper()))?.ParamValue;
            string text = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP03.ToUpper()))?.ParamValue;

            MailMessage msg = new MailMessage();
            msg.Subject = "Thông tin mật khẩu";
            msg.From = new MailAddress(fromaddr);
            msg.Body = string.Format(text, email, passwordHash);
            msg.To.Add(new MailAddress(toaddr));
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.EnableSsl = true;
            NetworkCredential nc = new NetworkCredential(fromaddr, password);
            smtp.Credentials = nc;

            try
            {
                smtp.Send(msg);
            }
            catch (SmtpFailedRecipientException ex)
            {

            }
        }

        public string GenPassWord()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz@#$&ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            var result = new string(
                Enumerable.Repeat(chars, 8)
                          .Select(s => s[random.Next(s.Length)])
                          .ToArray());
            return result;
        }

        /// <summary>
        /// Quên mật khẩu 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task ForgotPassword(string email)
        {
            var learnerInfo = sqlContext.Learner.Where(a => a.Email.Contains(email) && a.Provider.Equals("Email")).FirstOrDefault();

            if (learnerInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Email);
            }


            var password = GenPassWord();
            var securityStamp = passwordUtilsService.CreatePasswordHash();
            learnerInfo.SecurityStamp = securityStamp;
            learnerInfo.PasswordHash = passwordUtilsService.ComputeHash(password + securityStamp);
            learnerInfo.IsLogin = true;

            sqlContext.SaveChanges();
            await SendEmail(email, password);
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="model">
        /// 1: Password cũ
        /// 2: Password mới</param>
        /// <param name="learnerid">id người dùng</param>
        /// <returns></returns>
        public async Task ChangePass(ChangePasswordFrontendModel model, string learnerid)
        {
            // Lấy thông tin user
            var user = sqlContext.Learner.FirstOrDefault(r => r.Id.Equals(learnerid));

            if (!user.PasswordHash.Equals(passwordUtilsService.ComputeHash(model.PasswordOld + user.SecurityStamp)))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0047, TextResourceKey.User);
            }

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            if (string.IsNullOrEmpty(model.PasswordNew))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018, TextResourceKey.PassWordNew);

            }

            if (string.IsNullOrEmpty(model.PasswordOld))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018, TextResourceKey.PassWordOld);

            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(model.PasswordNew) && hasUpperChar.IsMatch(model.PasswordNew) && hasLowerChar.IsMatch(model.PasswordNew) && hasSymbols.IsMatch(model.PasswordNew);

            if (isValidated == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0045, TextResourceKey.User);
            }

            user.SecurityStamp = passwordUtilsService.CreatePasswordHash();
            user.IsLogin = false;
            user.PasswordHash = passwordUtilsService.ComputeHash(model.PasswordNew + user.SecurityStamp);

            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Đổi mật khẩu khi login lần đầu
        /// </summary>
        /// <param name="model">
        /// 1: Mật khẩu cũ</param>
        /// <param name="learnerid">Id người dùng</param>
        /// <returns></returns>
        public async Task ResetPassword(ResetPasswordFrontEndModel model, string learnerid)
        {
            var user = sqlContext.Learner.FirstOrDefault(r => r.Id.Equals(learnerid));

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }


            if (string.IsNullOrEmpty(model.PasswordNew))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0018, TextResourceKey.PassWordNew);

            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(model.PasswordNew) && hasUpperChar.IsMatch(model.PasswordNew) && hasLowerChar.IsMatch(model.PasswordNew) && hasSymbols.IsMatch(model.PasswordNew);

            if (isValidated == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0045, TextResourceKey.User);
            }

            user.SecurityStamp = passwordUtilsService.CreatePasswordHash();
            user.IsLogin = false;
            user.PasswordHash = passwordUtilsService.ComputeHash(model.PasswordNew + user.SecurityStamp);

            await sqlContext.SaveChangesAsync();
        }

        public async Task<UserLoginModel> GetGoogleUserDataAsync(HttpRequest request, GoogleModel model)
        {
            HttpClient client = new HttpClient();
            //var urlProfile = "https://www.googleapis.com/oauth2/v1/userinfo?access_token=" + access_token;
            var urlProfile = "https://oauth2.googleapis.com/tokeninfo?id_token=" + model.Id_Token;
            client.CancelPendingRequests();
            HttpResponseMessage output = await client.GetAsync(urlProfile);

            if (output.IsSuccessStatusCode)
            {
                string outputData = await output.Content.ReadAsStringAsync();
                GoogleUserModel userStatus = JsonConvert.DeserializeObject<GoogleUserModel>(outputData);

                if (userStatus != null)
                {
                    var userInfo = GetUserInfo(model.Id);

                    //var userInfo = (from a in sqlContext.Learner.AsNoTracking()
                    //                where a.Id.Equals(model.Id)
                    //                select new UserLoginModel
                    //                {
                    //                    Id = a.Id,
                    //                    DateOfBirthday = a.DateOfBirthday,
                    //                    PhoneNumber = a.PhoneNumber,
                    //                    DistrictId = a.DistrictId,
                    //                    NationId = a.NationId,
                    //                    ProvinceId = a.ProvinceId,
                    //                    WardId = a.WardId,
                    //                    access_token = a.Token,
                    //                    IdToken = a.IdToken,
                    //                    Name = a.Name,
                    //                    Email = a.Email,
                    //                    Avatar = a.Avatar,
                    //                    Provider = a.Provider,
                    //                    IsDisable = a.IsDisable
                    //                }).FirstOrDefault();



                    if (userInfo != null)
                    {
                        UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userInfo.Id);
                        userHistory.Content = "Tài khoản: " + userInfo.Name + " đã đăng nhập bằng google";
                        userHistory.Type = 1;

                        LogService.Event(userHistory, _detection);
                        return userInfo;

                    }
                    else
                    {
                        using (var trans = sqlContext.Database.BeginTransaction())
                        {
                            try
                            {
                                string file = string.Empty;
                                if (userStatus.picture != null)
                                {
                                    using (WebClient clients = new WebClient())
                                    {
                                        var filename = Path.GetExtension(userStatus.picture);
                                        string pathFolder = Path.Combine("fileUpload", "Register\\Google");
                                        string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
                                        // Kiểm tra folder upload
                                        if (!Directory.Exists(pathFolderServer))
                                        {
                                            Directory.CreateDirectory(pathFolderServer);
                                        }

                                        if (!string.IsNullOrEmpty(filename))
                                        {
                                            file = pathFolder + "\\" + userStatus.Email + filename;
                                        }
                                        else
                                        {
                                            file = pathFolder + "\\" + userStatus.Email + ".jpg";
                                        }
                                        clients.DownloadFile(new Uri(userStatus.picture), pathFolderServer + "\\" + userStatus.Email + filename);
                                    }
                                }

                                Learner learner = new Learner()
                                {
                                    Id = model.Id,
                                    IdToken = model.Id_Token,
                                    Name = userStatus.Name,
                                    Email = userStatus.Email,
                                    Avatar = file,
                                    IsDisable = false,
                                    Provider = "GOOGLE",
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                };

                                sqlContext.Learner.Add(learner);

                                sqlContext.SaveChanges();
                                trans.Commit();

                                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, learner.Id);
                                userHistory.Content = "Tài khoản: " + learner.Name + " đã được tạo bằng google";
                                userHistory.Type = 1;

                                LogService.Event(userHistory, _detection);

                                var user = GetUserInfo(model.Id);


                                userHistory.Content = "Tài khoản: " + user.Name + " đã đăng nhập bằng google";
                                userHistory.Type = 1;


                                LogService.Event(userHistory, _detection);
                                return user;

                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                }
            }
            else
            {
                throw NTSException.CreateInstance("Đăng nhập không thành công!");
            }
        }

        public UserLoginModel GetUserInfo(string id)
        {
            var user = (from a in sqlContext.Learner.AsNoTracking()
                        where a.Id.Equals(id)
                        select new UserLoginModel
                        {
                            Id = a.Id,
                            DateOfBirthday = a.DateOfBirthday,
                            PhoneNumber = a.PhoneNumber,
                            DistrictId = a.DistrictId,
                            NationId = a.NationId,
                            ProvinceId = a.ProvinceId,
                            WardId = a.WardId,
                            access_token = a.Token,
                            IdToken = a.IdToken,
                            Name = a.Name,
                            Email = a.Email,
                            Avatar = a.Avatar,
                            Provider = a.Provider,
                            IsDisable = a.IsDisable

                        }).FirstOrDefault();

            if (user != null)
            {

                user.ListCourse = (from a in sqlContext.LearnerCourse.AsNoTracking()
                                   where a.LearnerId.Equals(id)
                                   join b in sqlContext.Course.AsNoTracking() on a.CourseId equals b.Id
                                   select new CourseLear
                                   {
                                       Id = b.Id,
                                       Name = b.Name,
                                   }).ToList();
            }

            return user;
        }

        public async Task<bool> Logout(HttpRequest request, string id)
        {

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, id);

            var user = sqlContext.Learner.Where(a => a.Id.Equals(id)).FirstOrDefault();

            userHistory.Content = "Tài khoản: " + user.Name + " đã đăng xuất";
            userHistory.Type = 1;
            LogService.Event(userHistory, _detection);

            return true;
        }

        public async Task<UserLoginModel> GetFacebookProfileAsync(HttpRequest request, string access_token)
        {
            UserLoginModel facebookProfile = new UserLoginModel();
            HttpClient client = new HttpClient();
            var urlProfile = "https://graph.facebook.com/me?fields=id,name,picture,email,first_name,last_name&access_token="
                + access_token;
            client.CancelPendingRequests();
            HttpResponseMessage output = await client.GetAsync(urlProfile);

            if (output.IsSuccessStatusCode)
            {
                string outputData = await output.Content.ReadAsStringAsync();
                facebookProfile = JsonConvert.DeserializeObject<UserLoginModel>(outputData);

                if (facebookProfile != null)
                {
                    //var userInfo = (from a in sqlContext.Learner.AsNoTracking()
                    //                where a.Id.Equals(facebookProfile.Id)
                    //                select new UserLoginModel
                    //                {
                    //                    Id = a.Id,
                    //                    DateOfBirthday = a.DateOfBirthday,
                    //                    PhoneNumber = a.PhoneNumber,
                    //                    DistrictId = a.DistrictId,
                    //                    NationId = a.NationId,
                    //                    ProvinceId = a.ProvinceId,
                    //                    WardId = a.WardId,
                    //                    access_token = a.Token,
                    //                    IdToken = a.IdToken,
                    //                    Name = a.Name,
                    //                    Email = a.Email,
                    //                    Avatar = a.Avatar,
                    //                    Provider = a.Provider,
                    //                    IsDisable = a.IsDisable

                    //                }).FirstOrDefault();

                    var userInfo = GetUserInfo(facebookProfile.Id);


                    if (userInfo != null)
                    {

                        UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userInfo.Id);
                        userHistory.Content = "Tài khoản: " + userInfo.Name + " đã đăng nhập bằng facebook";
                        userHistory.Type = 1;

                        LogService.Event(userHistory, _detection);

                        return userInfo;
                    }
                    else
                    {
                        using (var trans = sqlContext.Database.BeginTransaction())
                        {
                            try
                            {
                                string file = string.Empty;
                                using (WebClient clients = new WebClient())
                                {
                                    var filename = ".jpg";
                                    string pathFolder = Path.Combine("fileUpload", "Register\\Facebook");
                                    string pathFolderServer = Path.Combine(Directory.GetCurrentDirectory(), pathFolder);
                                    // Kiểm tra folder upload
                                    if (!Directory.Exists(pathFolderServer))
                                    {
                                        Directory.CreateDirectory(pathFolderServer);
                                    }
                                    file = pathFolder + "\\" + facebookProfile.Email + filename;
                                    clients.DownloadFile(new Uri(facebookProfile.picture.Data.Url), pathFolderServer + "\\" + facebookProfile.Email + filename);
                                }

                                Learner learner = new Learner()
                                {
                                    Id = facebookProfile.Id,
                                    Token = access_token,
                                    IsDisable = false,
                                    IdToken = facebookProfile.IdToken,
                                    Name = facebookProfile.Name,
                                    Email = facebookProfile.Email,
                                    Avatar = file,
                                    Provider = "FACEBOOK",
                                    CreateDate = DateTime.Now,
                                    UpdateDate = DateTime.Now,
                                };

                                sqlContext.Learner.Add(learner);

                                sqlContext.SaveChanges();
                                trans.Commit();

                                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, learner.Id);
                                userHistory.Content = "Tài khoản: " + learner.Name + " đã được tạo bằng facebook";
                                userHistory.Type = 1;

                                LogService.Event(userHistory, _detection);

                                var user = GetUserInfo(facebookProfile.Id);

                                userHistory.Content = "Tài khoản: " + user.Name + " đã đăng nhập bằng facebook";
                                userHistory.Type = 1;

                                LogService.Event(userHistory, _detection);

                                return user;
                            }
                            catch (Exception ex)
                            {
                                trans.Rollback();
                                throw ex;
                            }
                        }
                    }
                }
                else
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                }
            }
            else
            {
                throw NTSException.CreateInstance("Đăng nhập không thành công!");
            }
        }

        public async Task MobileUpdateUser(MobileUpdateUserMobile model, string id)
        {
            var data = sqlContext.Learner.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Learer);
            }

            data.PhoneNumber = model.PhoneNumber;
            data.Avatar = model.Avatar;
            data.DistrictId = model.DistrictId;
            data.NationId = model.NationId;
            data.ProvinceId = model.ProvinceId;
            data.WardId = model.WardId;
            data.DateOfBirthday = model.DateOfBirthday;
            data.Gender = model.Gender;
            data.Address = model.Address;
            data.Name = model.Name;
            data.Email = model.Email;
            data.UpdateDate = DateTime.Now;


            await sqlContext.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(UserLoginModel model, string id)
        {

            var data = sqlContext.Learner.Where(x => x.Id.Equals(id)).FirstOrDefault();

            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Learer);
            }

            data.PhoneNumber = model.PhoneNumber;
            data.Avatar = model.Avatar;
            data.DistrictId = model.DistrictId;
            data.NationId = model.NationId;
            data.ProvinceId = model.ProvinceId;
            data.WardId = model.WardId;
            data.DateOfBirthday = model.DateOfBirthday;
            data.Gender = model.Gender;
            data.Address = model.Address;
            data.Name = model.Name;
            data.Email = model.Email;
            data.UpdateDate = DateTime.Now;


            await sqlContext.SaveChangesAsync();

        }
        public async Task<UserLoginModel> GetUserByIdAsync(string id, int value)
        {
            var userInfo = (from a in sqlContext.Learner.AsNoTracking()
                            where a.Id.Equals(id)
                            join b in sqlContext.Nation.AsNoTracking() on a.NationId equals b.Id into ab
                            from ba in ab.DefaultIfEmpty()
                            join c in sqlContext.Province.AsNoTracking() on a.ProvinceId equals c.ProvinceId into ac
                            from ca in ac.DefaultIfEmpty()
                            join d in sqlContext.District.AsNoTracking() on a.DistrictId equals d.DistrictId into ad
                            from da in ad.DefaultIfEmpty()
                            join e in sqlContext.Ward.AsNoTracking() on a.WardId equals e.WardId into ae
                            from ea in ae.DefaultIfEmpty()
                            select new UserLoginModel
                            {
                                Id = a.Id,
                                DateOfBirthday = a.DateOfBirthday,
                                PhoneNumber = a.PhoneNumber,
                                DistrictId = a.DistrictId,
                                DistrictName = da.Name,
                                ProvinceName = ca.Name,
                                NationName = ba.Name,
                                WardName = ea.Name,
                                NationId = a.NationId,
                                ProvinceId = a.ProvinceId,
                                WardId = a.WardId,
                                access_token = a.Token,
                                IdToken = a.IdToken,
                                Name = a.Name,
                                Email = a.Email,
                                Avatar = a.Avatar,
                                Provider = a.Provider,
                                Address = a.Address,
                                Gender = a.Gender,
                                IsDisable = a.IsDisable
                            }).FirstOrDefault();
            if (userInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Learer);
            }

            if (value == 1)
            {
                //var isCheck = userInfo.Avatar.Contains(appSettingModel.ServerApiUrl);
                if (userInfo.Avatar == null)
                {
                    userInfo.Avatar = userInfo.Avatar;
                }
                else
                {
                    userInfo.Avatar = appSettingModel.ServerApiUrl + userInfo.Avatar;
                }
            }
            if (value == 2)
            {
                userInfo.Avatar = userInfo.Avatar;
            }

            userInfo.ListCourse = (from a in sqlContext.LearnerCourse.AsNoTracking()
                                   where a.LearnerId.Equals(id)
                                   join b in sqlContext.Course.AsNoTracking() on a.CourseId equals b.Id
                                   select new CourseLear
                                   {
                                       Id = b.Id,
                                       Name = b.Name,
                                   }).ToList();

            return userInfo;
        }

        public async Task<SearchBaseResultModel<ComboboxModel>> GetListProvince()
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.Province.AsNoTracking()
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.ProvinceId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }

        public async Task<UserLoginModel> Login(LoginModel model, int value)
        {
            var learnerInfo = sqlContext.Learner.Where(a => a.Email.Contains(model.Email) && a.Provider.Equals("Email")).FirstOrDefault();
            if (learnerInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0051, TextResourceKey.Login);
            }

            if (learnerInfo.IsDisable == true)
            {
                var list = await sqlContext.SystemParams.ToListAsync();

                string email = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP05.ToUpper()))?.ParamValue;
                string text = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP04.ToUpper()))?.ParamValue;
                throw NTSException.CreateInstance(string.Format(text, email));
            }

            var securityStampTemp = passwordUtilsService.ComputeHash(model.Password + learnerInfo.SecurityStamp);
            if (!learnerInfo.PasswordHash.Equals(securityStampTemp))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0051, TextResourceKey.Login);
            }

            UserLoginModel userLogin = new UserLoginModel();

            userLogin.Name = learnerInfo.Name;
            userLogin.IsLogin = learnerInfo.IsLogin;
            userLogin.Email = learnerInfo.Email;
            userLogin.Id = learnerInfo.Id;
            userLogin.Provider = learnerInfo.Provider;

            if (value == 1)
            {
                if (learnerInfo.Provider.Equals("Email"))
                {
                    userLogin.Avatar = appSettingModel.ServerApiUrl + learnerInfo.Avatar;
                }
                else
                {
                    userLogin.Avatar = learnerInfo.Avatar;
                }
            }
            if (value == 2)
            {
                userLogin.Avatar = learnerInfo.Avatar;
            }

            return userLogin;

        }

        public async Task<SearchBaseResultModel<ComboboxIntModel>> GetListNation()
        {
            SearchBaseResultModel<ComboboxIntModel> searchResult = new SearchBaseResultModel<ComboboxIntModel>();
            var province = (from r in sqlContext.Nation.AsNoTracking()
                            orderby r.Name
                            select new ComboboxIntModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }


        public async Task<SearchBaseResultModel<ComboboxModel>> GetListDistrictByProvinceId(string ProvinceId)
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.District.AsNoTracking()
                            where r.ProvinceId.Equals(ProvinceId)
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.DistrictId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }


        public async Task<SearchBaseResultModel<ComboboxModel>> GetListWardByDistrictId(string DistrictId)
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.Ward.AsNoTracking()
                            where r.DistrictId.Equals(DistrictId)
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.WardId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }
    }
}

