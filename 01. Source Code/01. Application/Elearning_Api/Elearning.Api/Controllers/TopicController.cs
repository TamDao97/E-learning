using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Topic;
using Elearning.Models;
using Elearning.Services.Topics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/topics")]
    [ApiController]
    [NTSAuthorize]
    public class TopicController : BaseApiController
    {
        private readonly ITopicService topicService;
        public TopicController (ITopicService topicService)
        {
            this.topicService = topicService;
        }
        /// <summary>
        /// Tìm kiếm chủ đề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult> SearchTopic ([FromBody] TopicSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await topicService.SearchTopicAsync(modelSearch);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin chủ đề theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<TopicModel>>> GetTopicById ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await topicService.GetTopicByIdAsync(id, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm chủ đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateTopic ([FromBody] TopicModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await topicService.CreateTopicAsync(Request, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Cập nhập chủ đề
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateTopic ([FromRoute] string id, [FromBody] TopicModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await topicService.UpdateTopicAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa chủ đề
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteTopic ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await topicService.DeleteTopicByIdAsync(Request, id, userId);
            return Ok(apiResultModel);
        }
    }
}
