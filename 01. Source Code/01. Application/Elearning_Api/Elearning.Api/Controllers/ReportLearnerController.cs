using Elearning.Model.Models.ReportLearner;
using Elearning.Models;
using Elearning.Services.ReportLearner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/report-learner")]
    [ApiController]
    [NTSAuthorize]
    public class ReportLearnerController : BaseApiController
    {
        private readonly IReportLearnerService reportLearnerService;
        public ReportLearnerController(IReportLearnerService reportLearnerService)
        {
            this.reportLearnerService = reportLearnerService;
        }

        /// <summary>
        /// Thống kê người học
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("report-learner")]
        public async Task<ActionResult<ReportLearnerResultModel>> ReportLearner([FromBody] ReportLearnerSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await reportLearnerService.ReportLearner(modelSearch);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-excel")]
        public async Task<ActionResult<ExportFileModel>> ExportExcel([FromBody] ReportLearnerSearchConditionModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await reportLearnerService.ExportExcelAsync(model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xuất pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("export-pdf")]
        public async Task<ActionResult<ExportFileModel>> ExportPdf([FromBody] ReportLearnerSearchConditionModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await reportLearnerService.ExportPdfAsync(model);

            return Ok(apiResultModel);
        }
    }
}
