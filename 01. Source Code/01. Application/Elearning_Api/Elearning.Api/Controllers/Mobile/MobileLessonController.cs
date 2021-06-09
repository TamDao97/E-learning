using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Mobile.Service.Lesson;
using Elearning.Model.Models.Fontend.Course;
using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/lesson")]
    [ApiController]
    public class MobileLessonController : BaseApiController
    {
        private readonly IMobileLessonService mobileLesson;
        public MobileLessonController(IMobileLessonService mobileLesson)
        {
            this.mobileLesson = mobileLesson;
        }

        /// <summary>
        /// Lấy danh sách bài giảng theo id khóa học
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult<ApiResultModel>> SearchCourseByIdProgram(string id, string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileLesson.SearchLessonByCourseId(id, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// chi tiết bài học theo id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getLessonBylessonId")]
        public async Task<ActionResult<ApiResultModel>> GetLessonByCourseId (string lessonId, string learnerid, string courseId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileLesson.GetLessonByCourseId(lessonId,learnerid, courseId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// chi tiết bài học theo id
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getLessonFrameById")]
        public async Task<ActionResult<ApiResultModel>> GetLessonFrameById(string id, string learnerid, string courseId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileLesson.GetLessonFrameByIdAsync(id, learnerid, courseId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create-lesson-history")]
        public async Task<ActionResult<ApiResultModel>> CreateLessonHistory(LessonHistoryModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await mobileLesson.CreateLessonHistory(model);

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("create-lesson-frame-history")]
        public async Task<ActionResult<ApiResultModel>> CreateLessonFrameHistory(LessonFrameHistorysModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await mobileLesson.CreateLessonFrameHistory(model);

            return Ok(apiResultModel);
        }
    }
}
