using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.HomeLink;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.HomeLink;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace Elearning.Api.Controllers
{
    [Route("api/home-link")]
    [ApiController]
    [ValidateModel]
    public class HomeLinkController : BaseApiController
    {
        private readonly IHomeLinkService homeLinkService;
        private readonly IDetectionService _detection;
        public HomeLinkController (IHomeLinkService homeLinkService, IDetectionService _detection)
        {
            this.homeLinkService = homeLinkService;
            this._detection = _detection;
        }
        
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<HomeLinkResultModel>>> SearchProgram ([FromBody] HomeLinkSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await homeLinkService.SearchHomeLinkAsync(modelSearch);

            return Ok(apiResultModel);
        }
        
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<HomeLinkInfoModel>>> GetProgramById ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await homeLinkService.GetHomeLinkByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateProgram ([FromBody] HomeLinkModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeLinkService.CreateHomeLinkAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateProgram ([FromRoute] int id, [FromBody] HomeLinkModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeLinkService.UpdateHomeLinkAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteProgram ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await homeLinkService.DeleteHomeLinkByIdAsync(Request, id, userId);

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update-status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusCourse ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await homeLinkService.UpdateStatusHomeLinkAsync(Request, id, userId);

            return Ok(apiResultModel);
        }
    }
}
