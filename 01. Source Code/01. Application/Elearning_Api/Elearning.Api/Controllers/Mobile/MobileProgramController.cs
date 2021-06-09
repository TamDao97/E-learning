using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Mobile.Service.Program;
using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/program")]
    [ApiController]
    public class MobileProgramController : BaseApiController
    {
        private readonly IMobileProgramService programService;

        public MobileProgramController (IMobileProgramService programService)
        {
            this.programService = programService;
        }
        /// <summary>
        /// Danh sách chương trình học
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAllProgram")]
        public async Task<ActionResult<ApiResultModel>> GetAllProgram (string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await programService.GetListProgram(learnerId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách chương trình học
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getProgramDetailById")]
        public async Task<ActionResult<ApiResultModel>> GetProgramDetailById (string id, string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await programService.GetProgramDetailById(id, learnerId);

            return Ok(apiResultModel);
        }
    }
}
