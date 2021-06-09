using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.ManageUnit;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.ManagerUnit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace Elearning.Api.Controllers
{
    [Route("api/manage-unit")]
    [ApiController]
    [NTSAuthorize]
    public class ManageUnitController : BaseApiController
    {
        private readonly IManageUnitService manageUnitService;
        private readonly IDetectionService _detection;
        public ManageUnitController (IManageUnitService manageUnitService, IDetectionService _detection)
        {
            this.manageUnitService = manageUnitService;
            this._detection = _detection;
        }

        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<ManageUnitResultModel>>> SearchProgram ([FromBody] ManageUnitSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await manageUnitService.SearchManageUnitAsync(modelSearch);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<ManageUnitInfoModel>>> GetProgramById ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await manageUnitService.GetManageUnitByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateProgram ([FromBody] ManageUnitModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await manageUnitService.CreateManageUnitAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateProgram ([FromRoute] string id, [FromBody] ManageUnitModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await manageUnitService.UpdateManageUnitAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteProgram ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await manageUnitService.DeleteManageUnitByIdAsync(Request, id, userId);

            return Ok(apiResultModel);
        }
    }
}
