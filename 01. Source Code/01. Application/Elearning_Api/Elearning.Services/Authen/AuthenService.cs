using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using NTS.Common.Users;
using NTS.Common.Utils;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Elearning.Models.Settings;
using NTS.Common.RedisCache;
using Elearning.Models.Entities;
using Elearning.Model.Models.User.User;
using Elearning.Services.UserDevice;

namespace Elearning.Services.Authen
{
    public class AuthenService : IAuthenService
    {
        private readonly ElearningContext sqlContext;
        private readonly IPasswordUtilsService passwordUtilsService;
        private readonly AppSettingModel appSettingModel;
        private readonly INtsUserService ntsUserService;
        private readonly RedisCacheSettings redisCacheSetting;
        private readonly RedisCacheService redisCacheService;

        public AuthenService(ElearningContext sqlContext, IPasswordUtilsService passwordUtilsService, INtsUserService ntsUserService, RedisCacheService redisCacheService,
            IOptions<AppSettingModel> options, IOptions<RedisCacheSettings> redisOptions)
        {
            this.sqlContext = sqlContext;
            this.passwordUtilsService = passwordUtilsService;
            this.appSettingModel = options.Value;
            this.ntsUserService = ntsUserService;
            this.redisCacheSetting = redisOptions.Value;
            this.redisCacheService = redisCacheService;
        }

        public async Task<NtsUserTokenModel> LoginAsync(NtsLogInModel loginModel)
        {

            var user = (from u in sqlContext.User.AsNoTracking()
                        where u.UserName.Equals(loginModel.Username) && (u.Type == Constants.User_UserType_Admin || u.Type == Constants.User_UserType_Expert || u.Type == Constants.User_UserType_Instructor)
                        join e in sqlContext.Employee.AsNoTracking() on u.ObjectId equals e.Id
                        select new NtsUserLoginModel
                        {
                            UserId = u.Id,
                            PasswordHash = u.PasswordHash,
                            SecurityStamp = u.SecurityStamp,
                            UserName = u.UserName,
                            EmployeeId = e.Id,
                            IsLogin = u.IsLogin,
                            IsDisable = u.IsDisable,
                            ImageLink = e.Avatar,
                            Type = u.Type,
                            Name = e.Name,
                            ManagerUnitId = u.ManagerUnitId,
                            ManagerUnitLevel = u.ManagerUnitLevel,
                        }).FirstOrDefault();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0021, TextResourceKey.Login);
            }

            if (user.IsDisable == true)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0050, TextResourceKey.Account);

            }

            user.Permissions = GetListPermission(user.UserId);

            user.Password = loginModel.Password;
            user.ExpireDateAfter = appSettingModel.ExpireDateAfter;
            user.Secret = appSettingModel.Secret;

            NtsUserTokenModel userToken = await ntsUserService.NtsJwtLogin(user);

            return userToken;
        }

        /// <summary>
        /// Login cho người dùng
        /// </summary>
        /// <param name="loginModel">Dữ liệu tài khoản, mật khẩu truyền lên api</param>
        /// <returns></returns>
        public async Task<NtsUserTokenModel> LoginUserAsync(NtsLogInModel loginModel)
        {
            var user = (from u in sqlContext.User.AsNoTracking()
                        where u.UserName.Equals(loginModel.Username) &&  u.Type == Constants.User_UserType_Student
                        join e in sqlContext.Employee.AsNoTracking() on u.ObjectId equals e.Id
                        select new NtsUserLoginModel
                        {
                            UserId = u.Id,
                            PasswordHash = u.PasswordHash,
                            SecurityStamp = u.SecurityStamp,
                            UserName = u.UserName,
                            EmployeeId = e.Id,
                            IsLogin = u.IsLogin
                        }).FirstOrDefault();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0021, TextResourceKey.Login);
            }

            user.UserId = loginModel.UserId;
            user.Password = loginModel.Password;
            user.ExpireDateAfter = appSettingModel.ExpireDateAfter;
            user.Secret = appSettingModel.Secret;

            var userToken = await ntsUserService.NtsJwtLogin(user);

            return userToken;
        }

        public async Task<bool> LogOutAsync(string userId)
        {
            //Lấy cache 
            string key = $"{ redisCacheSetting.PrefixSystemKey }{redisCacheSetting.PrefixLoginKey}{userId}";

            ////Kiểm tra tồn tại
            //var register = await redisCacheService.ExistsAsync(key);
            //if (!register)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0019, TextResourceKey.User);
            //}

            //var userdevices = sqlcontext.userdevices.where(i => i.userid.equals(userid)).tolist();
            //if (userdevices.count > 0)
            //{
            //    sqlcontext.userdevices.removerange(userdevices);
            //}

            //sqlContext.SaveChanges();

            redisCacheService.Remove(key);
            return true;
        }

        /// <summary>
        /// Thêm mới user
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateUser(UserCreateModel model, string userId)
        {
            var userName = sqlContext.User.AsNoTracking().Where(a => a.UserName.Equals(model.UserName)).FirstOrDefault();
            var userEmail = sqlContext.Employee.AsNoTracking().Where(a => a.Email.Equals(model.Email)).FirstOrDefault();
            var userPhoneNumber = sqlContext.Employee.AsNoTracking().Where(a => a.PhoneNumber.Equals(model.PhoneNumber)).FirstOrDefault();

            if (userName != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
            }

            if (userEmail != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0043, TextResourceKey.User);
            }

            if (userPhoneNumber != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0044, TextResourceKey.User);
            }

            using (var trans = sqlContext.Database.BeginTransaction())
            {

                // Thêm mới thông tin nhân viên
                Employee employee = new Employee()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    PhoneNumber = model.PhoneNumber,
                    Address = model.Address,
                    Avatar = model.Avatar,
                    Birthday = model.Birthday,
                    Description = model.Description,
                    Email = model.Email,
                    Gender = model.Gender,
                    WorkUnit = model.WorkUnit,
                    CreateBy = userId,
                    UpdateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateDate = DateTime.Now,
                };

                sqlContext.Employee.Add(employee);

                // Thêm mới thông tin tài khoản

                if (!string.IsNullOrEmpty(model.UserName))
                {
                    User user = new User()
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserName = model.UserName,
                        ObjectId = employee.Id,
                        SecurityStamp = passwordUtilsService.CreatePasswordHash(),
                        IsDisable = model.IsDisable,
                        IsLogin = false,
                        GroupUserId = model.GroupUserId,
                        Type = model.Type,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };


                    user.PasswordHash = passwordUtilsService.ComputeHash(model.Password + user.SecurityStamp);
                    sqlContext.User.Add(user);
                }

                try
                {
                    sqlContext.SaveChanges();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public async Task LoginFacebook()
        {

        }

        public async Task LoginGoogle()
        {

        }

        public async Task<NtsUserTokenModel> GetUserInfoAsync(string accessToken)
        {
            NtsUserTokenModel userInfo = new NtsUserTokenModel();
            accessToken = accessToken.Replace("Bearer", "");
            // Key lưu cache login
            string keyLogin = $"{redisCacheSetting.PrefixSystemKey}{redisCacheSetting.PrefixLoginKey}{accessToken.Trim()}";

            try
            {
                userInfo = await redisCacheService.GetAsync<NtsUserTokenModel>(keyLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return userInfo;
        }

        public bool IsTokenAlive(string userId)
        {
            // Key lưu cache login
            string keyLogin = $"{redisCacheSetting.PrefixSystemKey}{redisCacheSetting.PrefixLoginKey}{userId}";
            bool isToken = false;

            try
            {
                isToken = redisCacheService.Exists(keyLogin);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return isToken;
        }

        public List<string> GetListPermission (string userId)
        {
            List<string> listPermission = new List<string>();

            var userPermission = (from a in sqlContext.User.AsNoTracking()
                                  where a.Id.Equals(userId)
                                  join b in sqlContext.UserPermission.AsNoTracking() on a.Id equals b.UserId
                                  join c in sqlContext.Permission.AsNoTracking() on b.PermissionId equals c.Id
                                  select new { c.Code, c.ScreenCode }
                                 ).ToList();

            listPermission = userPermission.Select(s => s.Code).ToList();
            listPermission.AddRange(userPermission.Where(r => !string.IsNullOrEmpty(r.ScreenCode)).GroupBy(g => g.ScreenCode).Select(s => s.Key).ToList());

            return listPermission;
        }
    }
}
