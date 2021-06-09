using Elearning.Model.Models.ReportLearnerProvince;
using Elearning.Models;
using Elearning.Services.ReportLearnerProvince;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/report-learner-province")]
    [ApiController]
    [NTSAuthorize]
    public class ReportLearnerProvinceController : BaseApiController
    {
        private readonly IReportLearnerProvinceService reportLearnerProvinceService;
        public ReportLearnerProvinceController(IReportLearnerProvinceService reportLearnerProvinceService)
        {
            this.reportLearnerProvinceService = reportLearnerProvinceService;
        }

        /// <summary>
        /// Thống kê thông tin người
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("report-learner-province")]
        public async Task<ActionResult<ReportLearnerProvinceResultModel>> ReportLearnerProvince(SearchReportLearnerModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await reportLearnerProvinceService.ReportLearnerProvince(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-file")]
        public async Task<ActionResult<string>> ExportFile([FromBody] ReportLearnerProvinceModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await reportLearnerProvinceService.ExportFileAsync(model);

            return Ok(apiResultModel);
        }
    }
}
