using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Elearning.Api.Attributes;
using Elearning.Models;
using Elearning.Services.Authen;
using NTS.Common.Users;
using Elearning.Models.Users;
using Elearning.Services.UserDevice;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.Log;
using Wangkanai.Detection.Services;

namespace Elearning.Api.Controllers
{
    [ApiController]
    [Route("api/authen")]
    public class AuthenController : BaseApiController
    {
        private readonly ILogger<AuthenController> _logger;
        private readonly IAuthenService authenService;
        private readonly IDetectionService _detection;

        public AuthenController(ILogger<AuthenController> logger, IAuthenService authenService, IDetectionService _detection)
        {
            _logger = logger;
            this.authenService = authenService;
            this._detection = _detection;
        }
       
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="logInModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ActionResult<ApiResultModel<UserTokenModel>>> LogInAsync([FromBody]NtsLogInModel logInModel)
        {

            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await authenService.LoginAsync(logInModel);
            UserHistoryModel model = UserDeviceService.GetUserHistory(Request, apiResultModel.Data);
            LogService.Login(model, _detection);
            return Ok(apiResultModel);

        }

        /// <summary>
        /// Đăng nhập tài khoản người dùng
        /// </summary>
        /// <param name="logInModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login-user")]
        public async Task<ActionResult<ApiResultModel<UserTokenModel>>> LoginUserAsync([FromBody] NtsLogInModel logInModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await authenService.LoginUserAsync(logInModel);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("logout")]
        public async Task<ActionResult<ApiResultModel>> LogOutAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string loginKey = GetUserIdByRequest();
            apiResultModel.Data = await authenService.LogOutAsync(loginKey);

            UserHistoryModel model = UserDeviceService.GetUserLogHistory(Request, loginKey);
            LogService.Logout(model, _detection);

            return Ok(apiResultModel);
        }
    }
}