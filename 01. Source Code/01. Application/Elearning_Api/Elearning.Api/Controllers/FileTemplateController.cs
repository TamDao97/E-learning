using Elearning.Api.Attributes;
using Elearning.Model.Models.FileTemplate;
using Elearning.Models;
using Elearning.Services.FileTemplate;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/file-template")]
    [ApiController]
    [ValidateModel]
    public class FileTemplateController : BaseApiController
    {
        private readonly IFileTemplateService service;
        public FileTemplateController(IFileTemplateService service)
        {
            this.service = service;
        }

        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("search/{type}")]
        public async Task<ActionResult<ApiResultModel>> Search([FromRoute] bool type)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await service.SearchAsync(type);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ApiResultModel>> Create(FileTemplateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await service.CreateAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update")]
        public async Task<ActionResult<ApiResultModel>> Update(FileTemplateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await service.UpdateAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa mẫu 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getById/{id}")]
        public async Task<ActionResult<ApiResultModel<Model.Entities.FileTemplate>>> GetById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await service.GetByIdAsync(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa mẫu 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<ApiResultModel>> delete([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await service.DeleteAsync(Request, id, userId);

            return Ok(apiResultModel);
        }
    }
}
