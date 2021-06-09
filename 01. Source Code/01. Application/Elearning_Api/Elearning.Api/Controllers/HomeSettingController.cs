using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.HomeSetting;
using Elearning.Models;
using Elearning.Services.HomeSetting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/home-setting")]
    [ApiController]
    [ValidateModel]
    public class HomeSettingController : BaseApiController
    {
        private readonly IHomeSettingService homeSettingService;
        public HomeSettingController (IHomeSettingService homeSettingService)
        {
            this.homeSettingService = homeSettingService;
        }
        [HttpGet]
        [Route("get-home-setting")]
        public async Task<ActionResult<ActionResult<HomeSettingModel>>> GetHomeSetting ()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await homeSettingService.GetHomeSettingAsync();

            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<HomeSettingModel>>> GetHomeSettingById ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await homeSettingService.GetHomeSettingByIdAsync(id, userId);

            return Ok(apiResultModel);
        }
        
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateHomeSetting ([FromBody] HomeSettingModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeSettingService.CreateHomeSettingAsync(Request, model, userId);

            return Ok(apiResultModel);
        }
        
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateHomeSetting ([FromRoute] int id, [FromBody] HomeSettingModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeSettingService.UpdateHomeSettingAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }
    }
}
