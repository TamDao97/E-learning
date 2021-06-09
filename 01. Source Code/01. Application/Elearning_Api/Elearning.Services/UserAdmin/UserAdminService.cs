using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.UserCustomer;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Models.Entities;
using NTS.Common.Utils;
using Elearning.Models.UserAdmin;
using Elearning.Model.Models.UserAdmins;
using System.Net.Mail;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Wangkanai.Detection.Services;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.UserDevice;
using Elearning.Services.Log;
using NTS.Common.RedisCache;
using Microsoft.Extensions.Options;

namespace Elearning.Services.UserCustomer
{
    public class UserAdminService : IUserAdminService
    {
        private readonly ElearningContext sqlContext;
        private readonly IPasswordUtilsService passwordUtilsService;
        private readonly IDetectionService _detection;
        private readonly RedisCacheSettings redisCacheSetting;
        private readonly RedisCacheService redisCacheService;

        public UserAdminService(ElearningContext sqlContext, IPasswordUtilsService passwordUtilsService, IDetectionService _detection, IOptions<RedisCacheSettings> redisOptions, RedisCacheService redisCacheService)
        {
            this.sqlContext = sqlContext;
            this.passwordUtilsService = passwordUtilsService;
            this._detection = _detection;
            this.redisCacheSetting = redisOptions.Value;
            this.redisCacheService = redisCacheService;
        }

        /// <summary>
        /// Tìm kiếm user quản trị
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserAdminResultModel>> SearchUserAdmin(UserAdminSearch searchModel)
        {
            SearchBaseResultModel<UserAdminResultModel> apiResultModel = new SearchBaseResultModel<UserAdminResultModel>();

            var data = (from a in sqlContext.User.AsNoTracking()
                        where a.Type == searchModel.Type
                        join b in sqlContext.Employee.AsNoTracking() on a.ObjectId equals b.Id
                        join c in sqlContext.ManagerUnit.AsNoTracking() on a.ManagerUnitId equals c.Id
                        orderby a.UserName
                        select new UserAdminResultModel()
                        {
                            Id = a.Id,
                            EmployeeId = b.Id,
                            UserName = a.UserName,
                            IsDisable = a.IsDisable,
                            Name = b.Name,
                            Gender = Constants.GetGender(b.Gender),
                            Birthday = b.Birthday,
                            PhoneNumber = b.PhoneNumber,
                            Email = b.Email,
                            Address = b.Address,
                            Avatar = b.Avatar,
                            WorkUnit = b.WorkUnit,
                            Description = b.Description,
                            ManagerUnitId = a.ManagerUnitId,
                            ManagerUnitLevel = a.ManagerUnitLevel,
                            ManagerUnitName = c.Name
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.UserName))
            {
                data = data.Where(a => a.UserName.ToUpper().Contains(searchModel.UserName.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ManagerUnitId))
            {
                data = data.Where(a => a.ManagerUnitId.Equals(searchModel.ManagerUnitId));
            }


            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                data = data.Where(a => a.PhoneNumber.ToUpper().Contains(searchModel.PhoneNumber.ToUpper()));
            }


            if (!string.IsNullOrEmpty(searchModel.WorkUnit))
            {
                data = data.Where(a => a.WorkUnit.ToUpper().Contains(searchModel.WorkUnit.ToUpper()));
            }


            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                data = data.Where(a => a.Email.ToUpper().Contains(searchModel.Email.ToUpper()));
            }


            if (searchModel.IsDisable.HasValue)
            {
                data = data.Where(a => a.IsDisable.Equals(searchModel.IsDisable));
            }

            int totalCount = data.Count();

            var listObject = data
                 .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                 .Take(searchModel.PageSize)
                 .ToList();

            apiResultModel.TotalItems = totalCount;
            apiResultModel.DataResults = listObject;

            return apiResultModel;
        }

        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<UserFontEndResultModel>> SearchUser(UserFondEndSearchModel searchModel)
        {
            SearchBaseResultModel<UserFontEndResultModel> apiResultModel = new SearchBaseResultModel<UserFontEndResultModel>();

            var data = (from a in sqlContext.Learner.AsNoTracking()
                        join b in sqlContext.Province.AsNoTracking() on a.ProvinceId equals b.ProvinceId into ab
                        from ba in ab.DefaultIfEmpty()
                        join c in sqlContext.District.AsNoTracking() on a.DistrictId equals c.DistrictId into ac
                        from ca in ac.DefaultIfEmpty()
                        join d in sqlContext.Ward.AsNoTracking() on a.WardId equals d.WardId into ad
                        from da in ad.DefaultIfEmpty()
                        join e in sqlContext.Nation.AsNoTracking() on a.NationId equals e.Id into ae
                        from ea in ae.DefaultIfEmpty()
                        orderby a.CreateDate descending
                        select new UserFontEndResultModel()
                        {
                            Id = a.Id,
                            Name = a.Name,
                            PhoneNumber = a.PhoneNumber,
                            DateOfBirthday = a.DateOfBirthday,
                            ProvinceId = ba.ProvinceId,
                            ProvinceName = ba.Name,
                            DistrictId = ca.DistrictId,
                            DistrictName = ca.Name,
                            WardId = da.WardId,
                            WardName = da.Name,
                            NationName = ea.Name,
                            NationId = ea.Id,
                            Avatar = a.Avatar,
                            Gender = a.Gender,
                            Email = a.Email,
                            Address = a.Address + " " + da.Name + " " + ca.Name + " " + ba.Name,
                            Provider = a.Provider,
                            IsDisable = a.IsDisable,
                            CreateDate = a.CreateDate
                        }).AsQueryable();


            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(a => a.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.Provider.ToString()))
            {
                if (searchModel.Provider == 1)
                {
                    data = data.Where(a => a.Provider.ToUpper().Contains("Google"));
                }

                if (searchModel.Provider == 2)
                {
                    data = data.Where(a => a.Provider.ToUpper().Contains("Facebook"));
                }

                if (searchModel.Provider == 3)
                {
                    data = data.Where(a => a.Provider.ToUpper().Contains("Email"));
                }
            }

            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                data = data.Where(a => a.Email.ToUpper().Contains(searchModel.Email.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ProvinceId))
            {
                data = data.Where(a => a.ProvinceId.Equals(searchModel.ProvinceId));
            }

            if (!string.IsNullOrEmpty(searchModel.DistrictId))
            {
                data = data.Where(a => a.DistrictId.Equals(searchModel.DistrictId));
            }

            if (!string.IsNullOrEmpty(searchModel.WardId))
            {
                data = data.Where(a => a.WardId.Equals(searchModel.WardId));
            }

            if (searchModel.NationId != null)
            {
                data = data.Where(a => a.NationId.Equals(searchModel.NationId));
            }


            if (searchModel.IsDisable.HasValue)
            {
                data = data.Where(a => a.IsDisable.Equals(searchModel.IsDisable));
            }

            if (searchModel.Old.HasValue)
            {
                var oldYear = DateTime.Now.Year;
                if (searchModel.Old == 1)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 18 && oldYear - a.DateOfBirthday.Value.Year <= 24);
                }

                if (searchModel.Old == 2)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 25 && oldYear - a.DateOfBirthday.Value.Year <= 34);
                }

                if (searchModel.Old == 3)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 35 && oldYear - a.DateOfBirthday.Value.Year <= 44);
                }

                if (searchModel.Old == 4)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 45 && oldYear - a.DateOfBirthday.Value.Year <= 54);
                }

                if (searchModel.Old == 5)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 55 && oldYear - a.DateOfBirthday.Value.Year <= 64);
                }

                if (searchModel.Old == 6)
                {
                    data = data.Where(a => a.DateOfBirthday != null && oldYear - a.DateOfBirthday.Value.Year >= 65);
                }

                if (searchModel.Old == 7)
                {
                    data = data.Where(a => a.DateOfBirthday == null);
                }
            }

            int totalCount = data.Count();

            var listObject = data
                 .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                 .Take(searchModel.PageSize)
                 .ToList();

            apiResultModel.TotalItems = totalCount;
            apiResultModel.DataResults = listObject;

            return apiResultModel;
        }

        /// <summary>
        /// Khóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UserAdminLock(HttpRequest request, string userId, string id)
        {
            var user = sqlContext.User.FirstOrDefault(r => r.Id.Equals(userId));
            var leared = sqlContext.Learner.FirstOrDefault(r => r.Id.Equals(userId));

            if (user == null && leared == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            UserHistoryModel userHistory = new UserHistoryModel();
            userHistory = UserDeviceService.GetUserLogHistory(request, id);
            string keyLogin = string.Empty;
            if (user != null)
            {
                user.IsDisable = true;
                keyLogin = $"{redisCacheSetting.PrefixSystemKey}{redisCacheSetting.PrefixLoginKey}{user.Id}";
                if (user.Type == 1)
                {
                    userHistory.Content = "Bạn đã khóa tài khoản quản trị hệ thống: " + user.UserName;
                }
                else if (user.Type == 2)
                {
                    userHistory.Content = "Bạn đã khóa tài khoản chuyên gia: " + user.UserName;
                }
                else if (user.Type == 3)
                {
                    userHistory.Content = "Bạn đã khóa tài khoản hướng dẫn viên: " + user.UserName;
                }
            }
            if (leared != null)
            {
                leared.IsDisable = true;
                keyLogin = $"{redisCacheSetting.PrefixSystemKey}{redisCacheSetting.PrefixLoginKey}{leared.Id}";
                userHistory.Content = "Bạn đã khóa tài khoản người dùng: " + leared.Name;
            }

            // Key lưu cache login
            var exist = await redisCacheService.ExistsAsync(keyLogin);
            if (exist)
            {
                redisCacheService.Remove(keyLogin);
            }

            sqlContext.SaveChanges();


            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Mở khóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UserAdminUnLock(HttpRequest request, string userId, string id)
        {
            var user = sqlContext.User.FirstOrDefault(r => r.Id.Equals(userId));
            var leared = sqlContext.Learner.FirstOrDefault(r => r.Id.Equals(userId));

            if (user == null && leared == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            UserHistoryModel userHistory = new UserHistoryModel();
            userHistory = UserDeviceService.GetUserLogHistory(request, id);


            if (user != null)
            {
                user.IsDisable = false;
                if (user.Type == 1)
                {
                    userHistory.Content = "Bạn đã mở khóa tài khoản quản trị hệ thống: " + user.UserName;
                }
                else if (user.Type == 2)
                {
                    userHistory.Content = "Bạn mở đã khóa tài khoản chuyên gia: " + user.UserName;
                }
                else if (user.Type == 3)
                {
                    userHistory.Content = "Bạn mở đã khóa tài khoản hướng dẫn viên: " + user.UserName;
                }
            }
            if (leared != null)
            {
                leared.IsDisable = false;
                userHistory.Content = "Bạn đã mở khóa tài khoản người dùng: " + leared.Name;

            }

            sqlContext.SaveChanges();
            LogService.Event(userHistory, _detection);

        }


        /// <summary>
        /// Lấy danh sách tài khoản theo id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<UserAdminModel> GetUserAdminById(string UserId)
        {
            var userId = sqlContext.User.Where(a => a.Id.Equals(UserId));

            if (userId == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            // Lấy thông tin user
            var resuldInfor = (from a in sqlContext.User.AsNoTracking()
                               where a.Id.Equals(UserId)
                               join b in sqlContext.Employee.AsNoTracking() on a.ObjectId equals b.Id
                               select new UserAdminModel()
                               {
                                   Id = a.Id,
                                   Email = b.Email,
                                   Address = b.Address,
                                   Avatar = b.Avatar,
                                   Birthday = b.Birthday,
                                   Description = b.Description,
                                   EmployeeId = b.Id,
                                   Gender = b.Gender,
                                   GroupUserId = a.GroupUserId,
                                   Type = a.Type,
                                   Name = b.Name,
                                   PhoneNumber = b.PhoneNumber,
                                   WorkUnit = b.WorkUnit,
                                   UserName = a.UserName,
                                   IsDisable = a.IsDisable,
                                   ManagerUnitLevel = a.ManagerUnitLevel,
                                   ManagerUnitId = a.ManagerUnitId,
                               }).FirstOrDefault();

            if (resuldInfor == null)
            {
                resuldInfor = new UserAdminModel();
            }

            // Lấy thông tin quyền
            List<GroupFunctionUserAdminModel> groupFunctions = new List<GroupFunctionUserAdminModel>();

            var gFunctions = sqlContext.GroupFunction.AsNoTracking().OrderBy(o => o.Index).ToList();

            var userPermissions = (from u in sqlContext.UserPermission.AsNoTracking()
                                   where u.UserId.Equals(resuldInfor.Id)
                                   select u.PermissionId).ToList();

            var permissions = (from p in sqlContext.Permission.AsEnumerable()
                               join u in userPermissions on p.Id equals u into pu
                               from pun in pu.DefaultIfEmpty()
                               orderby p.Code
                               select new PermissionsModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Code = p.Code,
                                   IsChecked = pun != null ? true : false,
                                   GroupFunctionId = p.GroupFunctionId
                               }).ToList();

            GroupFunctionUserAdminModel paramModel = new GroupFunctionUserAdminModel();

            foreach (var ite in gFunctions)
            {
                paramModel = new GroupFunctionUserAdminModel();
                paramModel.Id = ite.Id;
                paramModel.Name = ite.Name;
                paramModel.Permissions = permissions.Where(t => t.GroupFunctionId.Equals(ite.Id)).ToList();
                paramModel.PermissionTotal = paramModel.Permissions.Count;
                paramModel.IsChecked = paramModel.Permissions.Count(r => !r.IsChecked) == 0;
                paramModel.CheckCount = paramModel.Permissions.Count(r => r.IsChecked);
                groupFunctions.Add(paramModel);
            }

            resuldInfor.ListGroupFunction = groupFunctions;

            return resuldInfor;
        }

        /// <summary>
        /// Thêm mới user quản trị
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateAdminUser(HttpRequest request, UserCreateModel model, string userId)
        {
            var userName = sqlContext.User.AsNoTracking().Where(a => a.UserName.Trim().ToLower().Equals(model.UserName.Trim().ToLower())).FirstOrDefault();
            var userEmail = sqlContext.Employee.AsNoTracking().Where(a => a.Email.Trim().ToLower().Equals(model.Email.Trim().ToLower())).FirstOrDefault();
            var isCheckEmail = sqlContext.SystemParams.AsNoTracking().Where(a => a.ParamValue.Equals(Constants.EmailNoCheck)).FirstOrDefault();


            if (userName != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
            }

            if (userEmail != null && isCheckEmail == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0049, TextResourceKey.Email);
            }

            //if (userPhoneNumber != null)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0044, TextResourceKey.User);
            //}

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                UserHistoryModel userHistory = new UserHistoryModel();
                // Thêm mới thông tin nhân viên
                Employee employee = new Employee()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = StringHelper.NTSTrim(model.Name),
                    PhoneNumber = StringHelper.NTSTrim(model.PhoneNumber),
                    Address = StringHelper.NTSTrim(model.Address),
                    Avatar = model.Avatar,
                    Birthday = model.Birthday,
                    Description = StringHelper.NTSTrim(model.Description),
                    Email = StringHelper.NTSTrim(model.Email),
                    Gender = model.Gender,
                    WorkUnit = StringHelper.NTSTrim(model.WorkUnit),
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
                        UserName = StringHelper.NTSTrim(model.UserName),
                        ObjectId = employee.Id,
                        SecurityStamp = passwordUtilsService.CreatePasswordHash(),
                        IsDisable = model.IsDisable,
                        IsLogin = false,
                        GroupUserId = model.GroupUserId,
                        Type = model.Type,
                        ManagerUnitId = model.ManagerUnitId,
                        ManagerUnitLevel = model.ManagerUnitLevel,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now
                    };

                    user.PasswordHash = passwordUtilsService.ComputeHash(StringHelper.NTSTrim(model.Password) + user.SecurityStamp);
                    sqlContext.User.Add(user);


                    // Thêm mới bảng quyền
                    List<UserPermission> userPermissions = new List<UserPermission>();
                    UserPermission addPer;

                    foreach (var item in model.ListGroupFunction)
                    {
                        foreach (var per in item.Permissions)
                        {
                            if (per.IsChecked)
                            {
                                addPer = new UserPermission
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    UserId = user.Id,
                                    PermissionId = per.Id,
                                };

                                userPermissions.Add(addPer);
                            }
                        }
                    }

                    sqlContext.UserPermission.AddRange(userPermissions);
                }

                try
                {
                    await SendEmail(model.Email, model.UserName, model.Password);
                    sqlContext.SaveChanges();
                    trans.Commit();

                    userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Thêm mới nhân viên: " + model.Name;
                    LogService.Event(userHistory, _detection);


                    if (model.Type == 1)
                    {
                        userHistory.Content = "Thêm mới tài khoản admin: " + model.UserName;
                    }
                    else if (model.Type == 2)
                    {
                        userHistory.Content = "Thêm mới tài khoản chuyên gia: " + model.UserName;
                    }
                    else if (model.Type == 3)
                    {
                        userHistory.Content = "Thêm mới tài khoản hướng dẫn viên: " + model.UserName;
                    }

                    LogService.Event(userHistory, _detection);



                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task UpdateAdminUser(HttpRequest request, UserCreateModel model, string id, string userId)
        {
            var user = sqlContext.User.FirstOrDefault(r => r.Id.Equals(id));
            var employee = sqlContext.Employee.FirstOrDefault(r => r.Id.Equals(model.EmployeeId));
            string NameOld = employee.Name;
            var userName = sqlContext.User.AsNoTracking().Where(a => a.UserName.Equals(model.UserName) && !a.Id.Equals(id)).FirstOrDefault();
            var userEmail = sqlContext.Employee.AsNoTracking().Where(a => a.Email.Equals(model.Email) && !a.Id.Equals(model.EmployeeId)).FirstOrDefault();
            var isCheckEmail = sqlContext.SystemParams.AsNoTracking().Where(a => a.ParamValue.Equals(Constants.EmailNoCheck)).FirstOrDefault();

            if (userName != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.User);
            }

            if (userEmail != null && isCheckEmail == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0043, TextResourceKey.User);
            }

            var userPer = sqlContext.UserPermission.Where(r => r.UserId.Equals(id)).ToList();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            UserHistoryModel userHistory = new UserHistoryModel();

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                employee.Name = StringHelper.NTSTrim(model.Name);
                employee.PhoneNumber = StringHelper.NTSTrim(model.PhoneNumber);
                employee.Address = StringHelper.NTSTrim(model.Address);
                employee.Avatar = model.Avatar;
                employee.Birthday = model.Birthday;
                employee.Description = StringHelper.NTSTrim(model.Description);
                employee.Email = StringHelper.NTSTrim(model.Email);
                employee.Gender = model.Gender;
                employee.WorkUnit = StringHelper.NTSTrim(model.WorkUnit);
                employee.UpdateBy = userId;
                employee.UpdateDate = DateTime.Now;

                user.UserName = StringHelper.NTSTrim(model.UserName);
                user.IsDisable = model.IsDisable;
                user.GroupUserId = model.GroupUserId;
                user.ManagerUnitId = model.ManagerUnitId;
                user.ManagerUnitLevel = model.ManagerUnitLevel;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;

                if (userPer != null)
                {
                    sqlContext.UserPermission.RemoveRange(userPer);
                }

                // Thêm mới bảng quyền

                List<UserPermission> userPermissions = new List<UserPermission>();

                UserPermission addPer;

                foreach (var item in model.ListGroupFunction)
                {
                    foreach (var per in item.Permissions)
                    {
                        if (per.IsChecked)
                        {
                            addPer = new UserPermission
                            {
                                Id = Guid.NewGuid().ToString(),
                                UserId = user.Id,
                                PermissionId = per.Id,
                            };

                            userPermissions.Add(addPer);
                        }
                    }
                }
                sqlContext.UserPermission.AddRange(userPermissions);

                try
                {

                    userHistory = UserDeviceService.GetUserLogHistory(request, userId);

                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        userHistory.Content = "Cập nhật nhân viên là: " + NameOld;
                        LogService.Event(userHistory, _detection);

                    }
                    else
                    {
                        userHistory.Content = "Cập nhật  nhân viên có tên ban đầu là: " + NameOld + " thành " + model.Name;
                        LogService.Event(userHistory, _detection);

                    }

                    if (model.Type == 1)
                    {
                        userHistory.Content = "Cập nhật tài khoản admin: " + model.UserName;
                    }
                    else if (model.Type == 2)
                    {
                        userHistory.Content = "Cập nhật tài khoản chuyên gia: " + model.UserName;
                    }
                    else if (model.Type == 3)
                    {
                        userHistory.Content = "Cập nhật tài khoản hướng dẫn viên: " + model.UserName;
                    }
                    LogService.Event(userHistory, _detection);

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

        /// <summary>
        /// Xóa user
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public async Task DeleteUserAdmin(HttpRequest request, string id, string userid)
        {
            var userId = sqlContext.User.Find(id);
            var learner = sqlContext.Employee.Where(a => a.Id.Equals(userId.ObjectId)).FirstOrDefault();
            UserHistoryModel userHistory = new UserHistoryModel();
            //var leared = sqlContext.Learner.Find(id);
            var userPermission = sqlContext.UserPermission.Where(a => a.UserId.Equals(id)).ToList();

            var employeeCourse = sqlContext.EmployeeCourse.Where(a => a.EmployeeId.Equals(learner.Id)).ToList();

            if (userId == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                if (userPermission.Count > 0)
                {
                    sqlContext.RemoveRange(userPermission);
                }
                if (employeeCourse.Count > 0)
                {
                    sqlContext.RemoveRange(employeeCourse);
                }

                if (learner != null)
                {
                    sqlContext.Employee.Remove(learner);
                }

                if (userId != null)
                {
                    sqlContext.User.Remove(userId);
                }


                //if (leared != null)
                //{
                //    sqlContext.Learner.Remove(leared);
                //}

                userHistory = UserDeviceService.GetUserLogHistory(request, userid);


                userHistory.Content = "Xóa nhân viên: " + learner.Name;
                LogService.Event(userHistory, _detection);


                if (userId.Type == 1)
                {
                    userHistory.Content = "Xóa tài khoản admin: " + userId.UserName;

                }
                else if (userId.Type == 2)
                {
                    userHistory.Content = "Xóa tài khoản chuyên gia: " + userId.UserName;

                }
                else if (userId.Type == 3)
                {
                    userHistory.Content = "Xóa tài khoản hướng dẫn viên: " + userId.UserName;

                }

                try
                {
                    sqlContext.SaveChanges();
                    trans.Commit();
                    LogService.Event(userHistory, _detection);

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }

            }

        }

        /// <summary>
        /// Lấy quyền của nhóm theo id
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        public List<PermissionsModel> GetGroupPermissionById(string groupUserId)
        {
            var result = sqlContext.GroupPermission.AsNoTracking().Where(r => r.GroupUserId.Equals(groupUserId)).Select(
                s => new PermissionsModel
                {
                    Id = s.PermissionId
                }).ToList();

            return result;
        }

        /// <summary>
        /// Lấy quyền của nhóm
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        public List<GroupFunctionUserAdminModel> GetGroupPermission(string UserId)
        {

            List<GroupFunctionUserAdminModel> groupFunctions = new List<GroupFunctionUserAdminModel>();

            var gFunctions = sqlContext.GroupFunction.AsNoTracking().OrderBy(o => o.Index).ToList();

            var userPermissions = (from u in sqlContext.UserPermission.AsNoTracking()
                                   where u.UserId.Equals(UserId)
                                   select u.PermissionId).ToList();

            var permissions = (from p in sqlContext.Permission.AsEnumerable()
                               join u in userPermissions on p.Id equals u into pu
                               from pun in pu.DefaultIfEmpty()
                               orderby p.Code
                               select new PermissionsModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Code = p.Code,
                                   IsChecked = pun != null ? true : false,
                                   GroupFunctionId = p.GroupFunctionId,
                                   PermissionId = p.Id
                               }).ToList();

            GroupFunctionUserAdminModel paramModel = new GroupFunctionUserAdminModel();

            foreach (var ite in gFunctions)
            {
                paramModel = new GroupFunctionUserAdminModel();
                paramModel.Id = ite.Id;
                paramModel.Name = ite.Name;
                paramModel.Permissions = permissions.Where(t => t.GroupFunctionId.Equals(ite.Id)).ToList();
                paramModel.PermissionTotal = paramModel.Permissions.Count;
                paramModel.IsChecked = paramModel.Permissions.Count(r => !r.IsChecked) == 0;
                paramModel.CheckCount = paramModel.Permissions.Count(r => r.IsChecked);
                groupFunctions.Add(paramModel);
            }


            return groupFunctions.ToList();
        }

        public async Task ChangePassword(HttpRequest request, ChangePasswordModel model, string userId, string id)
        {
            var user = sqlContext.User.FirstOrDefault(r => r.Id.Equals(userId));


            var employee = (from a in sqlContext.User.AsNoTracking()
                            where a.Id.Equals(userId)
                            join b in sqlContext.Employee.AsNoTracking() on a.ObjectId equals b.Id
                            select b).FirstOrDefault();

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            if (employee == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(model.PasswordHash) && hasUpperChar.IsMatch(model.PasswordHash) && hasLowerChar.IsMatch(model.PasswordHash) && hasSymbols.IsMatch(model.PasswordHash);
            if (isValidated == false && model.isPassword == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0045, TextResourceKey.User);
            }

            user.SecurityStamp = passwordUtilsService.CreatePasswordHash();
            user.IsLogin = true;
            user.PasswordHash = passwordUtilsService.ComputeHash(model.PasswordHash + user.SecurityStamp);

            await SendEmail(employee.Email, user.UserName, model.PasswordHash);

            sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, id);


            if (user.Type == 1)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản admin: " + user.UserName;

            }
            else if (user.Type == 2)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản chuyên gia: " + user.UserName;

            }
            else if (user.Type == 3)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản hướng dẫn viên: " + user.UserName;

            }

            LogService.Event(userHistory, _detection);


        }

        public async Task ChangePass(HttpRequest request, ChangePass model, string userId)
        {
            var user = sqlContext.User.FirstOrDefault(r => r.Id.Equals(model.Id));

            var employee = (from a in sqlContext.User.AsNoTracking()
                            where a.Id.Equals(model.Id)
                            join b in sqlContext.Employee.AsNoTracking() on a.ObjectId equals b.Id
                            select b).FirstOrDefault();

            if (!user.PasswordHash.Equals(passwordUtilsService.ComputeHash(model.PasswordOld + user.SecurityStamp)))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0047, TextResourceKey.User);
            }

            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            if (employee == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");
            var hasSymbols = new Regex(@"[!@#$%^&*()_+=\[{\]};:<>|./?,-]");

            var isValidated = hasNumber.IsMatch(model.PasswordHash) && hasUpperChar.IsMatch(model.PasswordHash) && hasLowerChar.IsMatch(model.PasswordHash) && hasSymbols.IsMatch(model.PasswordHash);

            if (isValidated == false)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0045, TextResourceKey.User);
            }

            user.SecurityStamp = passwordUtilsService.CreatePasswordHash();
            user.IsLogin = true;
            user.PasswordHash = passwordUtilsService.ComputeHash(model.PasswordHash + user.SecurityStamp);

            sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);


            if (user.Type == 1)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản admin: " + user.UserName;

            }
            else if (user.Type == 2)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản chuyên gia: " + user.UserName;

            }
            else if (user.Type == 3)
            {
                userHistory.Content = "Thay đổi mật khẩu tài khoản hướng dẫn viên: " + user.UserName;

            }

            LogService.Event(userHistory, _detection);

        }

        private async Task SendEmail(string email, string username, string passwordHash)
        {
            var list = await sqlContext.SystemParams.ToListAsync();

            string fromaddr = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP01.ToUpper()))?.ParamValue;
            string toaddr = email;
            string password = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP02.ToUpper()))?.ParamValue;
            string text = list.FirstOrDefault(i => i.ParamName.ToUpper().Equals(Constants.SystemParam_SP03.ToUpper()))?.ParamValue;

            MailMessage msg = new MailMessage();
            msg.Subject = "Thông tin mật khẩu";
            msg.From = new MailAddress(fromaddr);
            msg.Body = string.Format(text, username, passwordHash);
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
    }
}


