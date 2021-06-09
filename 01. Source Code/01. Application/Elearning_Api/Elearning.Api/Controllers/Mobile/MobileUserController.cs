using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Fontend.User;
using Elearning.Model.Models.Mobile.Learner;
using Elearning.Model.Models.Mobile.User;
using Elearning.Model.Models.User.Learner;
using Elearning.Model.Models.User.User;
using Elearning.Models;
using Elearning.Services.Fontend.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/user")]
    [ApiController]
    public class MobileUserController : ControllerBase
    {
        private readonly ILoginService loginService;
        public MobileUserController(ILoginService loginService)
        {
            this.loginService = loginService;
        }

        /// <summary>
        /// Đổi mật khẩu
        /// </summary>
        /// <param name="model">
        /// 1: Password cũ
        /// 2: Password mới</param>
        /// <param name="learnerid">id người dùng</param>
        /// <returns></returns>
        [HttpPost]
        [Route("change-pass/{learnerid}")]
        public async Task<ActionResult<ApiResultModel>> ChangePass(ChangePasswordFrontendModel model, string learnerid)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await loginService.ChangePass(model, learnerid);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đổi mật khẩu khi login lần đầu
        /// </summary>
        /// <param name="model">
        /// 1: Mật khẩu cũ</param>
        /// <param name="learnerid">Id người dùng</param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset-password/{learnerid}")]
        public async Task<ActionResult<ApiResultModel>> ResetPassword(ResetPasswordFrontEndModel model, string learnerid)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await loginService.ResetPassword(model, learnerid);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Quên mật khẩu 
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forgot-pass/{email}")]
        public async Task<ActionResult<ApiResultModel>> ForgotPassword(string email)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await loginService.ForgotPassword(email);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Login email
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-email")]
        public async Task<ActionResult<ApiResultModel>> Login(LoginModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.Login(model, 1);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đăng nhập bằng goole
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-google")]
        public async Task<ActionResult<ApiResultModel<UserLoginModel>>> GetGoogleUserDataAsync(GoogleModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetGoogleUserDataAsync(Request, model);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Login facebook
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-facebook")]
        public async Task<ActionResult<ApiResultModel>> MobileGetFacebookProfile(FacebookModel model)
        {

            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetFacebookProfileAsync(Request, model.Id_Token);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("logout/{id}")]
        public async Task<ActionResult<ApiResultModel>> Logout(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await loginService.Logout(Request, id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin user theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<UserLoginModel>>> GetUserByIdAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetUserByIdAsync(id, 1);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật thông tin người dùng
        /// </summary>
        /// <param name="id">id người đăng nhập</param>
        /// <param name="model">Thông tin thay đổi truyền lên api</param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> Update([FromRoute] string id, [FromBody] MobileUpdateUserMobile model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await loginService.MobileUpdateUser(model, id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ApiResultModel>> CreateUserAsync(UserLearnerCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await loginService.CreateUserAsync(model);
            return Ok(apiResultModel);
        }
    }
}
