using Elearning.Model.Models.SystemParam;
using Elearning.Models;
using Elearning.Services.SystemParams;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/system-param")]
    [ApiController]
    [NTSAuthorize]
    public class SystemParamController : BaseApiController
    {
        private readonly ISystemParamService systemParamService;
        public SystemParamController(ISystemParamService systemParamService)
        {
            this.systemParamService = systemParamService;
        }

        /// <summary>
        /// Lấy danh sách thông số hệ thống
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<ApiResultModel<List<SystemParamModel>>>> GetListSystemParam()
        {
            ApiResultModel<List<SystemParamModel>> apiResultModel = new ApiResultModel<List<SystemParamModel>>
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await systemParamService.GetListSystemParamAsync();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật thông số hệ thống
        /// </summary>
        /// <param name="systemParams"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResultModel>> UpdateSystemParam([FromBody] List<SystemParamModel> systemParams)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await systemParamService.UpdateSystemParamAsync(systemParams);

            return Ok(apiResultModel);
        }
    }
}
