using Elearning.Model.Models.Fontend.User;
using Elearning.Model.Models.Mobile.Learner;
using Elearning.Model.Models.Mobile.User;
using Elearning.Model.Models.User.Learner;
using Elearning.Model.Models.User.User;
using Elearning.Model.Models.UserAdmins;
using Elearning.Models.Base;
using Elearning.Models.Combobox;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.User
{
    public interface ILoginService
    {
        Task CreateUserAsync(UserLearnerCreateModel model);


        /// <summary>
        /// Quên mật khẩu 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        Task ForgotPassword(string email);

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="model">
        /// 1: Password cũ
        /// 2: Password mới</param>
        /// <param name="learnerid">id người dùng</param>
        /// <returns></returns>
        Task ChangePass(ChangePasswordFrontendModel model, string learnerid);

        Task<UserLoginModel> Login(LoginModel model, int value);

        /// <summary>
        /// Đổi mật khẩu khi login lần đầu
        /// </summary>
        /// <param name="model">
        /// 1: Mật khẩu cũ</param>
        /// <param name="learnerid">Id người dùng</param>
        /// <returns></returns>
        Task ResetPassword(ResetPasswordFrontEndModel model, string learnerid);
        Task<UserLoginModel> GetGoogleUserDataAsync(HttpRequest request, GoogleModel model);
        Task<UserLoginModel> GetFacebookProfileAsync(HttpRequest request, string access_token);
        Task<bool> Logout(HttpRequest request, string id);
        Task UpdateUserAsync(UserLoginModel model, string id);
        Task MobileUpdateUser(MobileUpdateUserMobile model, string id);
        Task<UserLoginModel> GetUserByIdAsync(string id, int value);
        Task<SearchBaseResultModel<ComboboxModel>> GetListProvince();
        Task<SearchBaseResultModel<ComboboxIntModel>> GetListNation();
        Task<SearchBaseResultModel<ComboboxModel>> GetListDistrictByProvinceId(string ProvinceId);
        Task<SearchBaseResultModel<ComboboxModel>> GetListWardByDistrictId(string DistrictId);
    }
}
