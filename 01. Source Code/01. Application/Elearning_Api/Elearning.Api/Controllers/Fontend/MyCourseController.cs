using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Models;
using Elearning.Services.Fontend.MyCourse;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/my-course")]
    [ApiController]
    public class MyCourseController : BaseApiController
    {
        private readonly IMyCourseService myCourseService;
        public MyCourseController (IMyCourseService myCourseService)
        {
            this.myCourseService = myCourseService;
        }
        /// <summary>
        /// Lấy khóa học theo người học
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getMyCourse/{learnerId}")]
        public async Task<ActionResult> GetMyCourse ([FromRoute] string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await myCourseService.GetMyCourse(learnerId);

            return Ok(apiResultModel);
        }

    }
}
