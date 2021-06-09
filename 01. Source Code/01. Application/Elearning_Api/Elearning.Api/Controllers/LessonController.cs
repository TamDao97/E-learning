using Elearning.Api.Attributes;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Lesson;
using Elearning.Model.Models.Question;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Lessons;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/lessons")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class LessonController : BaseApiController
    {
        private readonly ILessonService lessonService;
        public LessonController(ILessonService lessonService)
        {
            this.lessonService = lessonService;
        }

        /// <summary>
        /// Tìm kiếm bài giảng
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<LessonSearchResultModel>>> SearchLesson([FromBody] LessonSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await lessonService.SearchLessonAsync(modelSearch, GetLevelRequest(), GetManageUnitRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới bài giảng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateLesson([FromBody] LessonCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await lessonService.CreateLessonAsync(Request, model, userId, GetManageUnitRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateLesson([FromRoute] string id, [FromBody] LessonCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await lessonService.UpdateLessonAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thay đổi trạng thái bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatus([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await lessonService.UpdateStatus(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel); 
        }

        /// <summary>
        /// Lấy thông tin bài giảng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<LessonCreateModel>>> GetLessonByIdAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await lessonService.GetLessonByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteLesson([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await lessonService.DeleteLessonByIdAsync(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm câu hỏi
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-question")]
        public async Task<ActionResult<SearchBaseResultModel<QuestionCreateModel>>> SaerchQuestion([FromBody] QuestionSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await lessonService.SaerchQuestion(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách câu hỏi ngẫu nhiên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-question-random")]
        public async Task<ActionResult<SearchBaseResultModel<QuestionCreateModel>>> GetListQuestionRandom([FromBody] QuestionRandomModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await lessonService.GetListQuestionRandom(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Yêu cầu duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request/{id}")]
        public async Task<ActionResult<ApiResultModel>> RequestLesson([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await lessonService.RequestLessonAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Duyệt, Không duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approval/{id}")]
        public async Task<ActionResult<ApiResultModel>> ApprovalLesson([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await lessonService.ApprovalLessonAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách lịch sử bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("approval-history/{id}")]
        public async Task<ActionResult<ApiResultModel>> GetListLessonApprovalStatus([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await lessonService.GetListLessonApprovalStatusAsync(id);

            return Ok(apiResultModel);
        }
    }
}
