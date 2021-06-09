using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.HomeSillder;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.SideBar;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/sildebars")]
    [ApiController]
    [ValidateModel]
    public class SideBarController : BaseApiController
    {
        private readonly ISideBarService sideBarService;
        public SideBarController(ISideBarService sideBarService)
        {
            this.sideBarService = sideBarService;
        }

        /// <summary>
        /// Tìm kiếm silder bar
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<HomeSilderResultModel>>> SearchHomeSliderAsync([FromBody] HomeSilderSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await sideBarService.SearchHomeSliderAsync(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Hiển thị lên trang chủ
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search-home")]
        public async Task<ActionResult<SearchBaseResultModel<HomeSilderResultModel>>> SearchSliderAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await sideBarService.SearchSliderAsync();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới silder bar
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateHomeSliderAsync([FromBody] HomeSilderCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.CreateHomeSliderAsync(Request, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật silder bar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateHomeSliderAsync([FromRoute] int id, [FromBody] HomeSilderCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.UpdateHomeSliderAsync(Request, id, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("update-index")]
        public async Task<ActionResult<ApiResultModel>> UpdateDisplayIndexAsync([FromBody] DisplayIndexModex model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.UpdateDisplayIndexAsync(Request, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin silder bar theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<HomeSilderModel>>> GetHomeSliderByIdAsync([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await sideBarService.GetHomeSliderByIdAsync(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa silder bar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteHomeSliderByIdAsync([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.DeleteHomeSliderByIdAsync(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }
    }
}
