using Microsoft.EntityFrameworkCore;
using NTS.Model.GroupUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Models.Base;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Models.Entities;
using Elearning.Models.GroupUser;
using Elearning.Model.Models.GroupUser;
using NTS.Common.RedisCache;
using Microsoft.Extensions.Options;

namespace Elearning.Services.GroupUsers
{
    public class GroupUserService : IGroupUserService
    {

        private readonly ElearningContext sqlContext;
        private readonly RedisCacheSettings redisCacheSetting;
        private readonly RedisCacheService redisCacheService;

        public GroupUserService(ElearningContext sqlContext, RedisCacheService redisCacheService, IOptions<RedisCacheSettings> redisOptions)
        {
            this.sqlContext = sqlContext;
            this.redisCacheService = redisCacheService;
            this.redisCacheSetting = redisOptions.Value;
        }

        public async Task AcceptFunction(string groupId, string userId)
        {
            var data = (from a in sqlContext.GroupPermission.AsNoTracking().Where(e => e.GroupUserId.Equals(groupId))
                        join b in sqlContext.Permission.AsNoTracking() on a.PermissionId equals b.Id
                        join c in sqlContext.UserPermission.AsNoTracking() on b.Id equals c.PermissionId
                        select c.UserId).Distinct().ToList();

            var functionId = (from e in sqlContext.GroupPermission.AsNoTracking().Where(g => g.GroupUserId.Equals(groupId))
                              select new
                              {
                                  e.PermissionId,
                              }).ToList();

            foreach (var item in data)
            {
                var userPermission = sqlContext.UserPermission.Where(a => a.UserId.Equals(item));

                if (userPermission.Count() > 0)
                {
                    sqlContext.UserPermission.RemoveRange(userPermission);
                }
                List<UserPermission> userPermissions = new List<UserPermission>();

                UserPermission userPer;

                foreach (var func in functionId)
                {
                    userPer = new UserPermission
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = item,
                        PermissionId = func.PermissionId,
                    };
                    userPermissions.Add(userPer);
                }

                sqlContext.UserPermission.AddRange(userPermissions);

            }

            await sqlContext.SaveChangesAsync();
        }
        /// <summary>
        /// Thêm nhóm quyền
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateGroupFunction(GroupFunctionCreateModel model, string userId)
        {
            //var isExistName = sqlContext.GroupUser.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.ToLower()));
            //if (isExistName != null)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.GroupUser);
            //}

            //using (var trans = sqlContext.Database.BeginTransaction())
            //{
            //    var dateNow = DateTime.Now;

            //    GroupUser data = new GroupUser()
            //    {
            //        Id = Guid.NewGuid().ToString(),
            //        Name = model.Name,
            //        IsDisable = model.IsDisable,
            //        Description = model.Description,
            //        CreateBy = userId,
            //        CreateDate = dateNow,
            //        UpdateBy = userId,
            //        UpdateDate = dateNow,
            //    };

            //    sqlContext.GroupUser.Add(data);

            //    List<GroupPermission> userPermissions = new List<GroupPermission>();

            //    GroupPermission addPer;
            //    foreach (var item in model.ListPermission)
            //    {
            //        foreach (var per in item.)
            //        {
            //            if (per.IsChecked)
            //            {
            //                addPer = new GroupPermission
            //                {
            //                    Id = Guid.NewGuid().ToString(),
            //                    GroupUserId = data.Id,
            //                    PermissionId = per.Id,
            //                };

            //                userPermissions.Add(addPer);
            //            }

            //        }
            //    }
            //    sqlContext.GroupPermission.AddRange(userPermissions);

            //    try
            //    {
            //        await sqlContext.SaveChangesAsync();
            //        trans.Commit();
            //    }
            //    catch (Exception ex)
            //    {
            //        trans.Rollback();
            //        throw ex;
            //    }
            //}
        }
        /// <summary>
        /// Xóa nhóm quyền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteGroupFunctionById(string id, string userId)
        {
            var data = sqlContext.GroupUser.FirstOrDefault(u => u.Id.Equals(id));
            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.GroupUser);
            }

            var groupPermissionOld = sqlContext.GroupPermission.Where(u => u.GroupUserId.Equals(id));
            if (groupPermissionOld.Count() > 0)
            {
                sqlContext.GroupPermission.RemoveRange(groupPermissionOld);
            }
            sqlContext.GroupUser.Remove(data);
            await sqlContext.SaveChangesAsync();
        }
        /// <summary>
        /// Lấy chức năng theo nhóm chức năng
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<FuntionModel>> GetFunctionByGroupFunctions(string groupId)
        {
            List<string> funtionSelect = new List<string>();
            SearchBaseResultModel<FuntionModel> searchResult = new SearchBaseResultModel<FuntionModel>();
            if (!string.IsNullOrEmpty(groupId))
            {
                funtionSelect = sqlContext.GroupPermission.Where(u => u.GroupUserId.Equals(groupId)).Select(u => u.PermissionId).ToList();
            }

            var groupFuntion = sqlContext.GroupFunction.ToList();
            var funtion = sqlContext.Permission.ToList();
            FuntionModel itemFuntion;
            List<FuntionModel> listFuntion = new List<FuntionModel>();
            List<FuntionModel> listFuntionTem;

            foreach (var item in groupFuntion)
            {
                itemFuntion = new FuntionModel();
                itemFuntion.Id = item.Id;
                itemFuntion.GroupFunctionId = "";
                itemFuntion.Code = item.Code;
                itemFuntion.Name = item.Name;
                listFuntion.Add(itemFuntion);
                listFuntionTem = (from a in funtion
                                  where a.GroupFunctionId.Equals(item.Id)
                                  select new FuntionModel
                                  {
                                      Id = a.Id,
                                      GroupFunctionId = item.Id,
                                      Name = a.Name,
                                      Code = a.Code,
                                      Checked = funtionSelect.Contains(a.Id)
                                  }).ToList();
                listFuntion.AddRange(listFuntionTem);
            }
            searchResult.TotalItems = funtion.Count;
            searchResult.DataResults = listFuntion;

            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được
            return searchResult;
        }
        /// <summary>
        /// Lấy chức năng theo nhóm và người dùng
        /// </summary>
        /// <param name="groupId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<FuntionModel>> GetFuntionByGroupAndUser(string groupId, string userId)
        {
            List<string> funtionSelect = new List<string>();
            SearchBaseResultModel<FuntionModel> searchResult = new SearchBaseResultModel<FuntionModel>();


            if (!string.IsNullOrEmpty(userId))
            {
                funtionSelect = sqlContext.UserPermission.Where(u => u.UserId.Equals(userId)).Select(u => u.PermissionId).ToList();
            }

            var listFuntion = (from a in sqlContext.Permission.AsNoTracking()
                               join b in sqlContext.GroupPermission.AsNoTracking() on a.Id equals b.PermissionId
                               where b.GroupUserId.Equals(groupId)
                               select new FuntionModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   Code = a.Code,
                                   Checked = funtionSelect.Contains(a.Id)
                               }).ToList();

            searchResult.DataResults = listFuntion;

            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được
            return searchResult;
        }
        /// <summary>
        /// Lấy nhóm quyền theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<GroupUserModel> GetGroupFunctionInfo(string id, string userId)
        {
            var groupUserModel = sqlContext.GroupUser.FirstOrDefault(u => u.Id.Equals(id));
            GroupUserModel model = new GroupUserModel();

            if (groupUserModel != null)
            {
                model.Id = groupUserModel.Id;
                model.Name = groupUserModel.Name;
                model.Description = groupUserModel.Description;
                var lstGroupPermissionsId = await sqlContext.GroupPermission.Where(u => u.GroupUserId.Equals(groupUserModel.Id)).Select(u => u.PermissionId).ToListAsync();
                model.ListPermission = GetListGroupPermissions(lstGroupPermissionsId);
            }
            else
            {
                model.Name = string.Empty; ;
                model.Status = false;
                model.Description = string.Empty;
                var lstGroupPermissionsId = new List<string>();
                model.ListPermission = GetListGroupPermissions(lstGroupPermissionsId);
            }

            return model;
        }

        public List<PermissionModel> GetListGroupPermissions(List<string> lstCheck)
        {
            List<PermissionModel> lst = new List<PermissionModel>();

            var lstPermissions = sqlContext.Permission.OrderBy(o => o.ScreenCode).ToList();
            List<Permission> lstPermissionsTemp;
            PermissionModel permissionModel;
            var group = sqlContext.GroupFunction.OrderBy(u => u.Index).ToList();
            for (int i = 0; i < group.Count; i++)
            {
                permissionModel = new PermissionModel();
                permissionModel.Index = (i + 1).ToString();
                permissionModel.FunctionCode = group[i].Code;
                permissionModel.FunctionName = group[i].Name;
                permissionModel.GroupFunctionId = string.Empty;
                permissionModel.FunctionId = group[i].Id;
                permissionModel.Checked = false;
                lst.Add(permissionModel);
                lstPermissionsTemp = lstPermissions.Where(u => u.GroupFunctionId.Equals(group[i].Id)).ToList();

                foreach (var item in lstPermissionsTemp)
                {
                    permissionModel = new PermissionModel();
                    permissionModel.Index = string.Empty;
                    permissionModel.FunctionCode = item.Code;
                    permissionModel.FunctionName = item.Name;
                    permissionModel.GroupFunctionId = group[i].Id;
                    permissionModel.FunctionId = item.Id;
                    permissionModel.Checked = lstCheck.Contains(item.Id);
                    permissionModel.ScreenCode = item.ScreenCode;
                    lst.Add(permissionModel);
                }
            }

            return lst;
        }


        /// <summary>
        /// Lấy nhóm chức năng
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        public List<PermissionModel> GetGroupPermission(string groupUserId)
        {
            var result = sqlContext.GroupPermission.AsNoTracking().Where(r => r.GroupUserId.Equals(groupUserId)).Select(
                s => new PermissionModel
                {
                    Id = s.PermissionId
                }).ToList();

            return result;
        }
        /// <summary>
        /// lấy chức năng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<GroupFunctionModel> GetPermisstions(string userId)
        {
            List<GroupFunctionModel> groupFunctions = new List<GroupFunctionModel>();

            var gFunctions = sqlContext.GroupFunction.AsNoTracking().ToList();

            var userPermissions = (from u in sqlContext.GroupPermission.AsNoTracking()
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
        /// Tìm kiếm nhóm quyền
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<GroupUserResultModel>> SearchGroupUser(GroupUserSearchModel modelSearch)
        {
            SearchBaseResultModel<GroupUserResultModel> searchResult = new SearchBaseResultModel<GroupUserResultModel>();
            var dataQuery = (from a in sqlContext.GroupUser.AsNoTracking()
                             orderby a.Name
                             select new GroupUserResultModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 IsDisable = a.IsDisable,
                                 Description = a.Description,

                             }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                dataQuery = dataQuery.Where(u => u.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (modelSearch.IsDisable.HasValue)
            {
                dataQuery = dataQuery.Where(u => u.IsDisable == modelSearch.IsDisable);
            }

            searchResult.TotalItems = dataQuery.Count();
            var listResult = await dataQuery.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize).Take(modelSearch.PageSize).ToListAsync();
            searchResult.DataResults = listResult;
            return searchResult;
        }

        /// <summary>
        /// Update nhóm quyền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateGroupFunction(string id, GroupFunctionCreateModel model, string updateby)
        {

            if (sqlContext.GroupUser.AsNoTracking().Where(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower())).Count() > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.GroupUser);
            }

            string nameOld = string.Empty;
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var groupUser = sqlContext.GroupUser.FirstOrDefault(u => u.Id.Equals(id));
                if (groupUser == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.GroupUser);
                }

                nameOld = groupUser.Name;
                groupUser.Name = model.Name;
                groupUser.Description = model.Description;
                groupUser.UpdateBy = updateby;
                groupUser.UpdateDate = DateTime.Now;

                //xóa quyền cũ
                var groupPermissionOld = sqlContext.GroupPermission.Where(u => u.GroupUserId.Equals(groupUser.Id));
                if (groupPermissionOld.Count() > 0)
                {
                    sqlContext.GroupPermission.RemoveRange(groupPermissionOld);
                }

                #region Thêm vào bảng con
                if (model.ListPermission.Count > 0)
                {
                    GroupPermission itemPermission;
                    foreach (var item in model.ListPermission.Where(u => u.Checked == true && string.IsNullOrEmpty(u.Index)))
                    {
                        itemPermission = new GroupPermission();
                        itemPermission.Id = Guid.NewGuid().ToString();
                        itemPermission.GroupUserId = groupUser.Id;
                        itemPermission.PermissionId = item.FunctionId;
                        sqlContext.GroupPermission.Add(itemPermission);
                    }
                }
                #endregion

                #region update quyền vào những tài khoản có nhóm quyền đó.
                var users = sqlContext.User.AsNoTracking().Where(a => a.GroupUserId.Equals(id)).Select(u => u.Id).ToList();

                var userpermision = sqlContext.UserPermission.AsNoTracking().ToList();

                string key = string.Empty;

                foreach (var userId in users)
                {
                    var userPermiss = userpermision.Where(a => a.UserId.Equals(userId)).ToList();
                    CreateUserPermission(userId, userPermiss, model.ListPermission);
                    

                }

                #endregion

                await sqlContext.SaveChangesAsync();
                await trans.CommitAsync();

                foreach(var userId in users)
                {
                    key = $"{ redisCacheSetting.PrefixSystemKey }{redisCacheSetting.PrefixLoginKey}{userId}";

                    ////Kiểm tra tồn tại

                    //var register = await redisCacheService.ExistsAsync(key);
                    //if (!register)
                    //{
                    //    throw NTSException.CreateInstance(MessageResourceKey.MSG0019, TextResourceKey.User);
                    //}

                    redisCacheService.Remove(key);
                }

            }

        }

        private void CreateUserPermission(string userId, List<UserPermission> listUserPermission, List<PermissionModel> listPermission)
        {
            if (listUserPermission.Count > 0)
            {
                sqlContext.UserPermission.RemoveRange(listUserPermission);
            }
            // Thêm mới bảng quyền

            List<UserPermission> userPermissions = new List<UserPermission>();

            UserPermission addPer;


            foreach (var item in listPermission.Where(u => u.Checked == true && string.IsNullOrEmpty(u.Index)))
            {
                addPer = new UserPermission
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = userId,
                    PermissionId = item.FunctionId,
                };

                userPermissions.Add(addPer);
            }

            sqlContext.UserPermission.AddRange(userPermissions);
        }
    }

}
