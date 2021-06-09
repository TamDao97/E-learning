using NTS.Model.GroupUser;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Models.Base;
using Elearning.Models.GroupUser;
using Elearning.Model.Models.GroupUser;

namespace Elearning.Services.GroupUsers
{
    public interface IGroupUserService
    {
        /// <summary>
        /// Tìm kiếm nhóm quyền
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<GroupUserResultModel>> SearchGroupUser (GroupUserSearchModel modelSearch);

        /// <summary>
        /// Thêm mới nhóm quyền
        /// </summary>
        /// <param name="model">Dữ liệu thêm mới</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task CreateGroupFunction (GroupFunctionCreateModel model, string userId);

        /// <summary>
        /// Xóa nhóm quyền
        /// </summary>
        /// <param name="id">Id bản ghi</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task DeleteGroupFunctionById (string id, string userId);

        /// <summary>
        /// lấy dữ liệu nhóm quyền
        /// </summary>
        /// <param name="id">Id bản ghi</param>
        /// <param name="userInfo">Thông tin đăng nhập</param>
        /// <returns></returns>
        Task<GroupUserModel> GetGroupFunctionInfo (string id, string userId);

        /// <summary>
        /// Cập nhật nhóm quyền
        /// </summary>
        /// <param name="id">id bản ghi</param>
        /// <param name="model">dữ liệu update</param>
        /// <param name="userInfo">thông tin người dùng đăng nhập</param>
        /// <returns></returns>
        Task UpdateGroupFunction (string id, GroupFunctionCreateModel model, string updateby);

        /// <summary>
        /// Lấy quyền
        /// </summary>
        /// <param name="groupId">id nhóm quyền</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<FuntionModel>> GetFunctionByGroupFunctions (string groupId);

        /// <summary>
        /// Lấy quyền theo nhóm quyền và tài khoản
        /// </summary>
        /// <param name="groupId">id nhóm quyền</param>
        /// <param name="userId">id user</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<FuntionModel>> GetFuntionByGroupAndUser (string groupId, string userId);

        /// <summary>
        /// Lấy nhóm quyền
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        List<PermissionModel> GetGroupPermission (string groupUserId);

        /// <summary>
        /// Lấy danh sách quyền
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        List<GroupFunctionModel> GetPermisstions (string userId);

        /// <summary>
        /// Lấy các quyền của nhóm quyền cập nhật cho các user có 1 trong các quyền thuộc nhóm.
        /// </summary>
        /// <param name="groupId">Id nhóm quyên</param>
        /// <param name="userInfo">Thông tin người dùng</param>
        /// <returns></returns
        Task AcceptFunction (string groupId, string userId);
    }
}
