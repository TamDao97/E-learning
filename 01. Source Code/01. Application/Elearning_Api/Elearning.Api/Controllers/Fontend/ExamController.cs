using Elearning.Model.Models.Comment;
using Elearning.Model.Models.Fontend.Exam;
using Elearning.Models;
using Elearning.Services.Fontend.Exam;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/exams")]
    [ApiController]
    public class ExamController : BaseApiController
    {
        private readonly IExamService examService;
        public ExamController(IExamService examService)
        {
            this.examService = examService;
        }

        /// <summary>
        /// Lấy thông tin bài giảng trắc nghiệm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<ExamModel>>> GetExamById([FromRoute] string id, CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.GetExamByIdAsync(id, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lưu bài giảng trắc nghiệm
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApistringResultModel>> CreateTest([FromBody] TestCreateModel model)
        {
            ApistringResultModel apiResultModel = new ApistringResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            userId = "12sdsd";
            apiResultModel.Data = await examService.CreateTestAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("getListQuestionAnswer")]
        public async Task<ActionResult> GetListQuestionAnswer([FromBody] ExamQuestionModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await examService.GetListQuestionAnswer(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lưu tạm đáp án câu hỏi trả lời
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns></returns>
        [HttpPost]
        [Route("save-temp")]
        public async Task<ActionResult<ApiResultModel>> CreateTest(SaveTempCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await examService.CreateTest(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// kết thúc bài thi trắc nghiệm
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        [HttpPost]
        [Route("finish-test")]
        public async Task<ActionResult<ApiResultModel>> CreateListTest([FromBody] FinishTestCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.CreateListTest(model);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin khóa học và danh sách bài giảng
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("lesson-course/{slug}")]
        public async Task<ActionResult<ActionResult<CommentCreateModel>>> GetLessonCourse([FromRoute] string slug, string userId, [FromBody] List<LessonCoursesModel> listCol)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.GetLessonCourseAsync(Request, slug, userId, listCol);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-lesson-id-by-slug")]
        public async Task<ActionResult<ApiResultModel>> GetAllProgram(string slug)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.GetLessonIdByslug(slug);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin danh mục bài giảng trắc nghiêm
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("exam-frame/{id}")]
        public async Task<ActionResult<ActionResult<ExamModel>>> GetExamFrameById([FromRoute] string id, CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.GetExamFrameByIdAsync(id, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Kết thúc bài thi trắc nghiệm danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        [HttpPost]
        [Route("finish-lesson-frame/{id}")]
        public async Task<ActionResult<ApiResultModel>> CreateListTest([FromRoute] string id, [FromBody] FinishTestCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await examService.CreateListLessonFrame(id, model);

            return Ok(apiResultModel);
        }
    }
}
