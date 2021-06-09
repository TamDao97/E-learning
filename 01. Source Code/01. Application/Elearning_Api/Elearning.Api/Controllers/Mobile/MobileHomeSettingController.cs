using System.Threading.Tasks;
using Elearning.Mobile.Service.HomeSetting;
using Elearning.Models;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/home-setting")]
    [ApiController]
    public class MobileHomeSettingController : BaseApiController
    {
        private readonly IMobileHomeSettingService homeSettingService;
        public MobileHomeSettingController (IMobileHomeSettingService homeSettingService)
        {
            this.homeSettingService = homeSettingService;
        }
        [HttpGet]
        [Route("get-home-setting")]
        public async Task<ActionResult<ApiResultModel>> GetHomeSetting ()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await homeSettingService.GetHomeSetting();

            return Ok(apiResultModel);
        }
    }
}
