using Elearning.Api.Attributes;
using Elearning.Model.Models.Category;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Authen;
using Elearning.Services.Categorys;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/categorys")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class CategoryController : BaseApiController
    {
        private readonly ICategoryService categoryService;
        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<CategorySearchResultModel>>> SearchCategory([FromBody] CategorySearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await categoryService.SearchCategoryAsync(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateCategory([FromBody] CategoryCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await categoryService.CreateCategoryAsync(model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateCategory([FromRoute] string id, [FromBody] CategoryCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await categoryService.UpdateCategoryAsync(id, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin danh mục theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<CategoryCreateModel>>> GetCategoryByIdAsync([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await categoryService.GetCategoryByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteCategory([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await categoryService.DeleteCategoryByIdAsync(id);

            return Ok(apiResultModel);
        }
    }
}
