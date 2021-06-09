using System.Threading.Tasks;
using Elearning.Model.Models.Mobile.Course;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Mobile.Course;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/course")]
    [ApiController]
    public class MobileCourseController : BaseApiController
    {
        private readonly IMobileCourseService mobileCourse;
        public MobileCourseController(IMobileCourseService mobileCourse)
        {
            this.mobileCourse = mobileCourse;
        }

        /// <summary>
        /// Lấy danh sách khóa học theo id chương trình đào tạo
        /// </summary>
        /// <param name="id">id chương trình đào tạo</param>
        /// <returns>
        /// 1: Trạng thái thành công/ thất bại
        /// 2: List khóa học
        /// </returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<ApiResultModel>> SearchCourseByIdProgram(string id,string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileCourse.SearchCourseByIdProgram(id, learnerId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy chi tiết khóa học theo id
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        [HttpGet]
        [Route("getcoursebyid")]
        public async Task<ActionResult<ApiResultModel>> GetInfoCourseById( string id, string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileCourse.GetInfoCourseById(id, learnerId);

            return Ok(apiResultModel);
        }

       
        [HttpPost]
        [Route("registerCourse")]
        public async Task<ActionResult<ApiResultModel>> CreateCourse ([FromBody] MobileLearnerCourseCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await mobileCourse.RegisterCourseAsync(model);
            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("searchCourse")]
        public async Task<ActionResult<ApiResultModel>> SearchCourse ([FromBody] MobileCourseSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

           apiResultModel.Data= await mobileCourse.SearchCourse(model);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách khóa học của tôi
        /// </summary>
        /// <param name="learnerId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getMyCourse")]
        public async Task<ActionResult<ApiResultModel>> GetAllMyCourse (string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileCourse.MyCourse(learnerId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách hướng dẫn viên theo khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getlistemployeecourse")]
        public async Task<ActionResult<ApiResultModel>> GetListEmployeeCourse(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileCourse.GetListEmployeeCourse(id);

            return Ok(apiResultModel);
        }
    }
}
