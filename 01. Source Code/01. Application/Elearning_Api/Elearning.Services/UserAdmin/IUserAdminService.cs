using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Model.Models.UserAdmins;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.UserAdmin;
using Elearning.Models.UserCustomer;
using Microsoft.AspNetCore.Http;

namespace Elearning.Services.UserCustomer
{
    public interface IUserAdminService
    {
        /// <summary>
        /// Timf kiếm tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<UserAdminResultModel>> SearchUserAdmin(UserAdminSearch model);

        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<UserFontEndResultModel>> SearchUser(UserFondEndSearchModel searchModel);

        /// <summary>
        /// Khóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UserAdminLock(HttpRequest request, string userId, string id);

        /// <summary>
        /// Mở khóa tài khoản
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>

        Task UserAdminUnLock(HttpRequest request, string userId, string id);

        /// <summary>
        /// Lấy dữ liệu tài khoản theo id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        Task<UserAdminModel> GetUserAdminById(string UserId);

        /// <summary>
        /// Thêm tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateAdminUser(HttpRequest request, UserCreateModel model, string userId);

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task UpdateAdminUser(HttpRequest request, UserCreateModel model, string id, string userId);

        /// <summary>
        /// Xóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task DeleteUserAdmin(HttpRequest request, string Id, string userId);

        /// <summary>
        /// Lấy quyền theo id nhóm userId
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        List<PermissionsModel> GetGroupPermissionById(string groupUserId);

        /// <summary>
        /// Lấy quyền theo user id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>

        List<GroupFunctionUserAdminModel> GetGroupPermission(string UserId);

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <param name="model">Mật khẩu truyền lên</param>
        /// <param name="userId">User id</param>
        /// <returns></returns>
        Task ChangePassword(HttpRequest request, ChangePasswordModel model, string userId, string id);

        Task ChangePass(HttpRequest request, ChangePass model, string userId);
    }
}
