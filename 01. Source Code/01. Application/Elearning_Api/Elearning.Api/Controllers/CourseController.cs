using System.Collections.Generic;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Fontend.Exam;
using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;
using Elearning.Model.Models.UserHistory;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Course;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace Elearning.Api.Controllers
{
    [Route("api/courses")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class CourseController : BaseApiController
    {
        private readonly ICourseService cousreService;
        public CourseController(ICourseService cousreService)
        {
            this.cousreService = cousreService;

        }

        /// <summary>
        /// Tìm kiếm hướng dẫn viên theo khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search-mentor/{id}")]
        public async Task<ActionResult<SearchBaseResultModel<MentorResultModel>>> SearchMentor([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.SearchMentor(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="searchModel">Thông tin tìm kiếm</param>
        /// <returns>Danh sách khóa học</returns>
        [Route("search")]
        [HttpPost]
        public async Task<ActionResult> SearchCourse([FromBody] CourseSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.SearchCourseAsync(searchModel, GetLevelRequest(), GetManageUnitRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteCourse([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await cousreService.DeleteCourseByIdAsync(Request, id, userId);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusCourse([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await cousreService.UpdateStatusCourseAsync(Request, id, userId);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách bài giảng cho khóa học
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("search-lesson")]
        [HttpPost]
        public async Task<ActionResult> GetListLesson([FromBody] LessonModelSearch searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetListLesson(searchModel);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<CourseInfoModel>>> GetCourseById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await cousreService.GetCourseByIdAsync(id);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateCourse([FromBody] CourseCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await cousreService.CreateCourseAsync(Request, model, userId, GetManageUnitRequest());



            return Ok(apiResultModel);
        }
        /// <summary>
        /// Cập nhập khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateCourse([FromRoute] string id, [FromBody] CourseCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await cousreService.UpdateCourseAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách bài giảng theo id khóa học
        /// </summary>
        /// <param name="courseId">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [Route("{courseId}/search-lesson-by-courseid")]
        [HttpGet]
        public async Task<ActionResult> SearchLessonByCourseId([FromRoute] string courseId)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.SearchLessonByCourseId(courseId);

            return Ok(apiResultModel);
        }

        /// <summary>
        ///  Tìm kiếm hướng dẫn viên theo khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search-learner/{id}")]
        public async Task<ActionResult<SearchBaseResultModel<LearnerResultModel>>> SearchLearner([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.SearchLearner(id);

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("progress")]
        public async Task<ActionResult<List<ProgressModel>>> GetProgess([FromBody] ProgressSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string employeeId = GetEmployeeIdByRequset();
            apiResultModel.Data = await cousreService.GetProgress(model);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-employee-course/{programId}")]
        public async Task<ActionResult<List<EmployeeCourseModel>>> GetEmployeeCourseAsync(string programId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string employeeId = GetEmployeeIdByRequset();

            apiResultModel.Data = await cousreService.GetEmployeeCourseAsync(programId, employeeId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("test-result")]
        public async Task<ActionResult<List<TestResultModel>>> GetTestResult([FromBody] TestResultSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetTestResult(model.CourseId, model.LearnerId);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("detail-result/{testId}")]
        public async Task<ActionResult<List<QuestionModel>>> GetQuestion(string testId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetQuestionByLessonId(testId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Danh sách giá trị thứ tự ưu tiên
        /// </summary>
        /// <returns></returns>
        [Route("getListOrder")]
        [HttpGet]
        public async Task<ActionResult> GetListOrder()
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetListOrder();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("file-templates")]
        public async Task<ActionResult<List<TestResultModel>>> GetFileTemplates()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await cousreService.GetFileTemplates();

            return Ok(apiResultModel);
        }

        [Route("print-certificate")]
        [HttpPost]
        public async Task<ActionResult<string>> PrintListCertificate([FromBody] CertificateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await cousreService.PrintCertificate(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Yêu cầu duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request/{id}")]
        public async Task<ActionResult<ApiResultModel>> RequestCourse([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await cousreService.RequestCourseAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Duyệt, Không duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approval/{id}")]
        public async Task<ActionResult<ApiResultModel>> ApprovalCourse([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await cousreService.ApprovalCourseAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách lịch sử khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("approval-histiry/{id}")]
        public async Task<ActionResult<ApiResultModel>> GetListApprovalStatus([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetListApprovalStatusAsync(id);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("exam/{id}")]
        public async Task<ActionResult<ActionResult<ExamModel>>> GetExamById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await cousreService.GetExamByIdAsync(id);

            return Ok(apiResultModel);
        }
    }
}
