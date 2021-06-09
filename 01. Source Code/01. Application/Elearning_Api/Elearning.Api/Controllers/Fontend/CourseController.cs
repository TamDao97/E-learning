using Elearning.Model.Models.Fontend.Course;
using Elearning.Models;
using Elearning.Services.Fontend.Course;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/fontend/course")]
    [ApiController]
    public class CourseController : BaseApiController
    {
        private readonly ICourseService courseService;

        public CourseController(ICourseService courseService)
        {
            this.courseService = courseService;
        }

        /// <summary>
        /// Lấy chi tiết khóa học
        /// </summary>
        /// <param name="id"></param>
        ///  <param name="learnerId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("")]
        public async Task<ActionResult<ApiResultModel>> GetCourseById([FromBody] CourseIdModel courseIdModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await courseService.GetCourseById(courseIdModel);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Load ra danh sách chương trình và khóa học của chương trình đó
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("searchCourse")]
        public async Task<ActionResult> SearchCourse (string learnerid,string searchValue)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await courseService.SearchCourseAsync(learnerid, searchValue);

            return Ok(apiResultModel);
        }
    }
}
