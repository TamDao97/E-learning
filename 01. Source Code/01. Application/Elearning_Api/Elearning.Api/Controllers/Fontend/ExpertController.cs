using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Models;
using Elearning.Services.Fontend.Expert;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/fontend/expert")]
    [ApiController]
    public class ExpertController : ControllerBase
    {
        private readonly IExpertService expertService;
        public ExpertController (IExpertService expertService)
        {
            this.expertService = expertService;
        }
        /// <summary>
        /// Load ra danh sách giảng viên
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> GetProgram ()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await expertService.GetExpertAsync();

            return Ok(apiResultModel);
        }
    }
}
