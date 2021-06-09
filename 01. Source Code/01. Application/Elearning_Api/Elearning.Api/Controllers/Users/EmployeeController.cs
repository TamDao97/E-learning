using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elearning.Api.Attributes;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.User.Employee;
using Elearning.Services.Users.Employee;
using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;

namespace Elearning.Api.Controllers.Users
{
    [Route("api/employee")]
    [ApiController]
    [ValidateModel]
    public class EmployeeController : BaseApiController
    {
        private readonly IEmployeeService employee;
        public EmployeeController (IEmployeeService _employee)
        {
            employee = _employee;
        }
        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<EmployeeResultModel>>> SearchEmployee ([FromBody] EmployeeSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await employee.SearchEmployeeAsync(modelSearch);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin nhân viên theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<EmployeeInfoModel>>> GetEmployeeById ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await employee.GetEmployeeByIdAsync(id, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateEmployee ([FromBody] EmployeeCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await employee.CreateEmployeeAsync(model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Câp nhập nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateEmployee ([FromRoute] string id, [FromBody] EmployeeCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await employee.UpdateEmployeeAsync(id, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteEmployee ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await employee.DeleteEmployeeByIdAsync(id, userId);
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy quyền của nhóm
        /// </summary>
        /// <param name="groupUserId">id group user truyên lên</param>
        /// <returns></returns>
        [Route("group-permission/{groupUserId}")]
        [HttpPost]
        public async Task<ActionResult> GetGroupPermission ([FromRoute] string groupUserId)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = employee.GetGroupPermission(groupUserId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách quyền
        /// </summary>
        /// <returns></returns>
        [Route("permisstion")]
        [HttpPost]
        public async Task<ActionResult> GetPermisstions ()
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = employee.GetPermisstions(userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm hướng dẫn viên
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-mentor")]
        public async Task<ActionResult<SearchBaseResultModel<EmployeeResultModel>>> SearchMentor([FromBody] EmployeeSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await employee.SearchMentorAsync(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm người học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-learner")]
        public async Task<ActionResult<SearchBaseResultModel<LearnerResultModel>>> SearchLearner([FromBody] EmployeeSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await employee.SearchLearnerAsync(modelSearch);

            return Ok(apiResultModel);
        }
    }
}
