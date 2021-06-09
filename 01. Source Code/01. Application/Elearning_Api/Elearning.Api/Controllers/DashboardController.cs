using Elearning.Model.Models.ReportLearner;
using Elearning.Models;
using Elearning.Services.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/dashboard")]
    [ApiController]
    [NTSAuthorize]
    public class DashboardController : BaseApiController
    {
        private readonly IDashboardService dashboardService;
        public DashboardController(IDashboardService dashboardService)
        {
            this.dashboardService = dashboardService;
        }

        /// <summary>
        /// Thông tin tổng hợp
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<object>> GetTotalAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await dashboardService.GetTotalAsync();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thống kê người học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register-course")]
        public async Task<ActionResult<ReportLearnerResultModel>> GetRegisterCourse([FromBody] ReportLearnerSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await dashboardService.GetRegisterCourse(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thống kê loại đăng ký
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-register")]
        public async Task<ActionResult<ReportLearnerResultModel>> GetRegister([FromBody] ReportLearnerSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await dashboardService.GetRegister(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thống kê tỉnh, thành phố có nhiều người học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("get-province")]
        public async Task<ActionResult<ReportLearnerResultModel>> GetProvince([FromBody] ReportLearnerSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await dashboardService.GetProvince(modelSearch);

            return Ok(apiResultModel);
        }
    }
}
