using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.HomeService;
using Elearning.Models;
using Elearning.Services.HomeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/home-service")]
    [ApiController]
    [ValidateModel]
    public class HomeServiceController : BaseApiController
    {
        private readonly IHomeServiceService homeService;
        public HomeServiceController (IHomeServiceService homeService)
        {
            this.homeService = homeService;
        }
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult> SearchHomeService ([FromBody] HomeServiceSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await homeService.SearchHomeServiceAsync(modelSearch);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<HomeServiceModel>>> GetHomeServiceById ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await homeService.GetHomeServiceByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateHomeService ([FromBody] HomeServiceModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeService.CreateHomeServiceAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateHomeService ([FromRoute] int id, [FromBody] HomeServiceModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeService.UpdateHomeServiceAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteHomeService ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await homeService.DeleteHomeServiceByIdAsync(Request, id, userId);
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Danh sách giá trị thứ tự ưu tiên
        /// </summary>
        /// <returns></returns>
        [Route("getListOrder")]
        [HttpGet]
        public async Task<ActionResult> GetListOrder ()
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await homeService.GetListOrder();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusHomeService ([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await homeService.UpdateStatusHomeServiceAsync(Request, id, userId);
            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update-index")]
        public async Task<ActionResult<ApiResultModel>> UpdateIndexHomeService ([FromBody] List<HomeServiceIndex> model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await homeService.UpdateIndexHomeServiceAsync(Request, model, userId);

            return Ok(apiResultModel);
        }
    }

}
