using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using NTS.Common.Utils;
using Elearning.Models.Entities;
using Elearning.Models.Base;
using Elearning.Models.GroupUser;
using Elearning.Models.User.Employee;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;

namespace Elearning.Services.Users.Employee
{
    public class EmployeeService : IEmployeeService
    {
        private readonly ElearningContext sqlContext;
        private readonly IPasswordUtilsService passwordUtilsService;

        public EmployeeService (ElearningContext sqlContext, IPasswordUtilsService passwordUtilsService)
        {
            this.sqlContext = sqlContext;
            this.passwordUtilsService = passwordUtilsService;
        }
        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateEmployeeAsync (EmployeeCreateModel employeeModel, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                Models.Entities.Employee employee = new Models.Entities.Employee()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = employeeModel.Name,
                    Gender = employeeModel.Gender,
                    Birthday = employeeModel.BirthDay,
                    Address = employeeModel.Address,
                    Email = employeeModel.Email,
                    PhoneNumber = employeeModel.PhoneNumber,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now,
                };

                AddUser(employee.Id, employeeModel.UserModel, userId);

                sqlContext.Employee.Add(employee);

                try
                {
                    await sqlContext.SaveChangesAsync();
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
        /// Thêm tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userModel"></param>
        /// <param name="userId"></param>
        private void AddUser (string id, UserModel userModel, string userId)
        {
            var user = sqlContext.User.FirstOrDefault(u => id.Equals(u.ObjectId));

            // Cập nhật tài khoản
            if (user != null)
            {
                var checkUser = sqlContext.User.FirstOrDefault(u => !id.Equals(u.ObjectId) && u.UserName.ToLower().Equals(userModel.UserName));
                if (checkUser != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Account);
                }

                user.IsDisable = userModel.IsDisable;
                user.IsLogin = userModel.IsLogin;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;
                var funtionOld = sqlContext.UserPermission.Where(u => u.UserId.Equals(user.Id));
                if (funtionOld.Count() > 0)
                {
                    sqlContext.UserPermission.RemoveRange(funtionOld);
                }
            }
            // Thêm mới tài khoản
            else
            {
                var checkUser = sqlContext.User.FirstOrDefault(u => u.UserName.ToLower().Equals(userModel.UserName));
                if (checkUser != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Account);
                }

                user = new User();
                user.Id = Guid.NewGuid().ToString();
                user.ObjectId = id;
                user.UserName = userModel.UserName;
                user.SecurityStamp = passwordUtilsService.CreatePasswordHash();
                user.PasswordHash = UserHelper.GeneratePassword(10, 10);
                user.IsDisable = userModel.IsDisable;
                user.IsLogin = false;
                user.CreateBy = userId;
                user.CreateDate = DateTime.Now;
                user.UpdateBy = userId;
                user.UpdateDate = DateTime.Now;
                user.PasswordHash = passwordUtilsService.ComputeHash("123456" + user.SecurityStamp);
                sqlContext.User.Add(user);
            }
            List<UserPermission> userPermissions = new List<UserPermission>();

            UserPermission addPer;
            foreach (var item in userModel.ListFuntion)
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
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeByIdAsync (string id, string userId)
        {
            var employeeExist = await sqlContext.Employee.FindAsync(id);
            if (employeeExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
            }

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var user = sqlContext.User.Where(u => u.ObjectId.Equals(id)).ToList();
                if (user.Count > 0)
                {
                    foreach (var item in user)
                    {
                        var userPermission = sqlContext.UserPermission.Where(s => s.UserId == item.Id).ToList();

                        sqlContext.UserPermission.RemoveRange(userPermission);
                    }
                    sqlContext.User.RemoveRange(user);
                }

                sqlContext.Employee.Remove(employeeExist);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// Lấy danh sách nhân viên theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<EmployeeInfoModel> GetEmployeeByIdAsync (string id, string userId)
        {
            var resuldInfor = await (from a in sqlContext.Employee.AsNoTracking()
                                     where a.Id.Equals(id)
                                     select new EmployeeInfoModel()
                                     {
                                         Id = a.Id,
                                         Name = a.Name,
                                         Gender = a.Gender,
                                         BirthDay = a.Birthday,
                                         Address = a.Address,
                                         Email = a.Email,
                                         PhoneNumber = a.PhoneNumber,
                                     }).FirstOrDefaultAsync();
            if (resuldInfor == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
            }

            var userModel = (from a in sqlContext.User.AsNoTracking()
                             where a.ObjectId.Equals(resuldInfor.Id)
                             select new UserModel()
                             {
                                 Id = a.Id,
                                 IsDisable = a.IsDisable,
                                 IsLogin = a.IsLogin,
                                 UserName = a.UserName,
                                 PasswordHash = a.PasswordHash,
                                 SercurityStamp = a.SecurityStamp,
                                 ObjectId = a.ObjectId,
                                 Type = Constants.User_UserType_Admin,
                             }).FirstOrDefaultAsync();

            if (userModel != null)
            {
                resuldInfor.UserModel = await userModel;
            }
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();

            var gFunctions = sqlContext.GroupFunction.AsNoTracking().ToList();

            var userPermissions = (from u in sqlContext.UserPermission.AsNoTracking()
                                   where u.UserId.Equals(resuldInfor.UserModel.Id)
                                   select u.PermissionId).ToList();

            var permissions = (from p in sqlContext.Permission.AsEnumerable()
                               join u in userPermissions on p.Id equals u into pu
                               from pun in pu.DefaultIfEmpty()
                               select new PermissionModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Code = p.Code,
                                   IsChecked = pun != null ? true : false,
                                   GroupFunctionId = p.GroupFunctionId
                               }).ToList();

            GroupFunctionModel paramModel = new GroupFunctionModel();
            foreach (var ite in gFunctions)
            {
                paramModel = new GroupFunctionModel();
                paramModel.Id = ite.Id;
                paramModel.Name = ite.Name;
                paramModel.Permissions = permissions.Where(t => t.GroupFunctionId.Equals(ite.Id)).ToList();
                paramModel.PermissionTotal = paramModel.Permissions.Count;
                paramModel.Checked = paramModel.Permissions.Count(r => r.IsChecked == true) == 0 ? false : true;
                paramModel.CheckCount = paramModel.Permissions.Count(r => r.IsChecked);
                groupFunctions.Add(paramModel);
            }

            resuldInfor.ListGroupFunction = groupFunctions;
            return resuldInfor;
        }
        public async Task<SearchBaseResultModel<EmployeeResultModel>> SearchEmployeeAsync (EmployeeSearchConditionModel searchModel)
        {
            var data = (from a in sqlContext.Employee.AsNoTracking()
                        join f in sqlContext.User.AsNoTracking() on a.Id equals f.ObjectId into af
                        from fa in af.DefaultIfEmpty()
                        select new EmployeeResultModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Gender = a.Gender,
                            BirthDay = a.Birthday,
                            PhoneNumber = a.PhoneNumber,
                            Email = a.Email,
                            Address = a.Address,
                            UserName = fa != null ? fa.UserName : string.Empty,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            if (searchModel.Status.HasValue)
            {
                data = data.Where(r => searchModel.Status.Equals(r.Status));
            }

            SearchBaseResultModel<EmployeeResultModel> searchResult = new SearchBaseResultModel<EmployeeResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            return searchResult;
        }
        /// <summary>
        /// Cập nhập nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateEmployeeAsync (string id, EmployeeCreateModel employeeModel, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var employee = await sqlContext.Employee.FindAsync(id);
                if (employee == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Employee);
                }
                employee.Name = employeeModel.Name;
                employee.Gender = employeeModel.Gender;
                employee.Birthday = employeeModel.BirthDay;
                employee.Email = employeeModel.Email;
                employee.PhoneNumber = employeeModel.PhoneNumber;
                employee.Address = employeeModel.Address;
                employee.UpdateBy = userId;
                employee.UpdateDate = DateTime.Now;
                AddUser(id, employeeModel.UserModel, userId);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                }
            }
        }
        /// <summary>
        /// Lấy quyền của nhóm
        /// </summary>
        /// <param name="groupUserId">id group user truyên lên</param>
        /// <returns></returns>
        public List<PermissionModel> GetGroupPermission (string groupUserId)
        {
            var result = sqlContext.GroupPermission.AsNoTracking().Where(r => r.GroupUserId.Equals(groupUserId)).Select(
                s => new PermissionModel
                {
                    Id = s.PermissionId
                }).ToList();

            return result;
        }

        /// <summary>
        /// Lấy danh sách quyền
        /// </summary>
        /// <param name="userId">Thông tin người đăng nhập</param>
        /// <returns></returns>
        public List<GroupFunctionModel> GetPermisstions (string userId)
        {
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();

            var gFunctions = sqlContext.GroupFunction.AsNoTracking().ToList();

            var userPermissions = (from u in sqlContext.UserPermission.AsNoTracking()
                                   select u.PermissionId).Distinct().ToList();

            var permissions = (from p in sqlContext.Permission.AsEnumerable()
                               join u in userPermissions on p.Id equals u into pu
                               from pun in pu.DefaultIfEmpty()
                               select new PermissionModel
                               {
                                   Id = p.Id,
                                   Name = p.Name,
                                   Code = p.Code,
                                   IsChecked = false,
                                   GroupFunctionId = p.GroupFunctionId
                               }).ToList();

            GroupFunctionModel paramModel = new GroupFunctionModel();
            foreach (var ite in gFunctions)
            {
                paramModel = new GroupFunctionModel();
                paramModel.Id = ite.Id;
                paramModel.Name = ite.Name;
                paramModel.Permissions = permissions.Where(t => t.GroupFunctionId.Equals(ite.Id)).ToList();
                paramModel.PermissionTotal = paramModel.Permissions.Count;
                paramModel.Checked = paramModel.Permissions.Count(r => r.IsChecked == true) == 0 ? false : true;
                paramModel.CheckCount = paramModel.Permissions.Count(r => r.IsChecked);
                groupFunctions.Add(paramModel);
            }

            return groupFunctions;
        }

        /// <summary>
        /// Lấy danh sách mentor
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<EmployeeResultModel>> SearchMentorAsync(EmployeeSearchConditionModel searchModel)
        {
            var userMentor = sqlContext.User.AsNoTracking().Where(r => r.Type == 3);
            var data = (from a in userMentor
                        join f in sqlContext.Employee.AsNoTracking() on a.ObjectId equals f.Id
                        join c in sqlContext.User.AsNoTracking() on f.Id equals c.ObjectId
                        join d in sqlContext.ManagerUnit.AsNoTracking() on c.ManagerUnitId equals d.Id
                        where (a.IsDisable == false)
                        select new EmployeeResultModel
                        {
                            Id = f.Id,
                            Name = f.Name,
                            Gender = f.Gender,
                            BirthDay = f.Birthday,
                            PhoneNumber = f.PhoneNumber,
                            Email = f.Email,
                            Address = f.Address,
                            UserName = a != null ? a.UserName : string.Empty,
                            ManageUnit=d.Name,
                            Logo=d.Logo
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Address))
            {
                data = data.Where(r => r.Address.ToUpper().Contains(searchModel.Address.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                data = data.Where(r => r.Email.ToUpper().Contains(searchModel.Email.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                data = data.Where(r => r.PhoneNumber.ToUpper().Contains(searchModel.PhoneNumber.ToUpper()));
            }

            SearchBaseResultModel<EmployeeResultModel> searchResult = new SearchBaseResultModel<EmployeeResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách người học
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<LearnerResultModel>> SearchLearnerAsync(EmployeeSearchConditionModel searchModel)
        {
            var data = (from  f in sqlContext.Learner.AsNoTracking()
                        select new LearnerResultModel
                        {
                            Id = f.Id,
                            Name = f.Name,
                            PhoneNumber = f.PhoneNumber,
                            Email = f.Email,
                            Address = f.Address,
                            UserName = f != null ? f.Name : string.Empty,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Address))
            {
                data = data.Where(r => r.Address.ToUpper().Contains(searchModel.Address.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Email))
            {
                data = data.Where(r => r.Email.ToUpper().Contains(searchModel.Email.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.PhoneNumber))
            {
                data = data.Where(r => r.PhoneNumber.ToUpper().Contains(searchModel.PhoneNumber.ToUpper()));
            }

            SearchBaseResultModel<LearnerResultModel> searchResult = new SearchBaseResultModel<LearnerResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            return searchResult;
        }
    }
}
