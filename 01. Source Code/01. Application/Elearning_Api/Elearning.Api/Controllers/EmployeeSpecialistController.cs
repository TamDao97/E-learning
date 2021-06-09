using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.EmployeeSpecialist;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.EmployeeSpecialist;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/employee-specialists")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class EmployeeSpecialistController : BaseApiController
    {
        private readonly IEmployeeSpecialistService sideBarService;
        public EmployeeSpecialistController(IEmployeeSpecialistService sideBarService)
        {
            this.sideBarService = sideBarService;
        }

        /// <summary>
        /// Tìm kiếm chuyên gia
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<HomeSpecialistResultModel>> SearchEmployeeSpecialistAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await sideBarService.SearchEmployeeSpecialistAsync();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới chuyên gia
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateEmployeeSpecialistAsync([FromBody] EmployeeSpecialistCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.CreateEmployeeSpecialistAsync(Request, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật chuyên gia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateEmployeeSpecialistAsync([FromRoute] int id, [FromBody] EmployeeSpecialistCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.UpdateEmployeeSpecialistAsync(Request, id, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin chuyên gia theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<EmployeeSpecialistModel>> GetEmployeeSpecialistByIdAsync([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await sideBarService.GetEmployeeSpecialistByIdAsync(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa chuyên gia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteEmployeeSpecialistByIdAsync([FromRoute] int id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await sideBarService.DeleteEmployeeSpecialistByIdAsync(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        [HttpPost]
        [Route("employee-search")]
        public async Task<ActionResult<SearchBaseResultModel<EmployeeSpecialResultModel>>> SearchEmployee([FromBody] EmployeeSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await sideBarService.SearchEmployee(modelSearch);

            return Ok(apiResultModel);
        }
    }
}
