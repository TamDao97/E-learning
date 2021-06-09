using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Mobile.Service.Test;
using Elearning.Model.Models.Mobile.MobileTest;
using Elearning.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/test")]
    [ApiController]
    public class MobileTestController : ControllerBase
    {

        private readonly IMobileTestService mobileTest;
        public MobileTestController(IMobileTestService mobileTest)
        {
            this.mobileTest = mobileTest;
        }

        /// <summary>
        /// Lưu tạm đáp án câu hỏi trả lời
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns></returns>
        [HttpPost]
        [Route("save-temp")]
        public async Task<ActionResult<ApiResultModel>> CreateTest(MobileTestCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await mobileTest.CreateTest(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// kết thúc bài thi trắc nghiệm
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        [HttpPost]
        [Route("finish-test")]
        public async Task<ActionResult<ApiResultModel>> CreateListTest([FromBody] MobileTestCreateListModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileTest.CreateListTest(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// kết thúc bài thi trắc nghiệm bài giảng chi tiết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        [HttpPost]
        [Route("finish-test-lesson-frame/{id}")]
        public async Task<ActionResult<ApiResultModel>> CreateListTestLessonFrame([FromRoute] string id, [FromBody] MobileTestCreateListModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileTest.CreateListTestLessonFrame(id, model);

            return Ok(apiResultModel);
        }
    }
}
