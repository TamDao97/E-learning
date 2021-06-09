using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Fontend.User;
using Elearning.Model.Models.Mobile.Learner;
using Elearning.Model.Models.User.Learner;
using Elearning.Model.Models.User.User;
using Elearning.Model.Models.UserAdmins;
using Elearning.Models;
using Elearning.Models.Combobox;
using Elearning.Services.Fontend.User;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/fontend/userlogins")]
    [ApiController]
    public class UserLogin : ControllerBase
    {

        private readonly ILoginService loginService;
        public UserLogin(ILoginService loginService)
        {
            this.loginService = loginService;
        }

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


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResultModel>> Login(LoginModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.Login(model, 2);
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("get-google")]
        public async Task<ActionResult<ApiResultModel<UserLoginModel>>> GetGoogleUserDataAsync(GoogleModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetGoogleUserDataAsync(Request, model);
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("get-facebook")]
        public async Task<ActionResult<ApiResultModel>> GetFacebookProfile(FacebookModel model)
        {
            
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetFacebookProfileAsync(Request, model.Id_Token);
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("mobile-get-facebook")]
        public async Task<ActionResult<ApiResultModel>> MobileGetFacebookProfile(FacebookModel model)
        {

            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetFacebookProfileAsync(Request, model.Id_Token);
            return Ok(apiResultModel);
        }

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

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateLesson([FromRoute] string id, [FromBody] UserLoginModel model )
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await loginService.UpdateUserAsync(model, id );

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<UserLoginModel>>> GetUserByIdAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await loginService.GetUserByIdAsync(id, 2);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-province")]
        public async Task<ActionResult<ComboboxModel>> GetListProvince()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await loginService.GetListProvince();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-nation")]
        public async Task<ActionResult<ComboboxModel>> GetListNation()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await loginService.GetListNation();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-district/{provinceId}")]
        public async Task<ActionResult<ComboboxModel>> GetListDistrictByProvinceId(string provinceId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await loginService.GetListDistrictByProvinceId(provinceId);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-ward/{districtId}")]
        public async Task<ActionResult<ComboboxModel>> GetListWardByDistrictId(string districtId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await loginService.GetListWardByDistrictId(districtId);

            return Ok(apiResultModel);
        }

    }
}
