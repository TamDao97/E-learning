using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.CompleteStatistics;
using Elearning.Models;
using Elearning.Services.CompleteStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/complete-statistic")]
    [ApiController]
    [NTSAuthorize]
    public class CompleteStatisticsController : BaseApiController
    {
        private readonly ICompleteStatisticsService statisticsService;
        public CompleteStatisticsController (ICompleteStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpPost]
        [Route("")]
        public async Task<ActionResult> CompleteStatistic (SearchCompleteModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await statisticsService.StatisticalCompleteCourse(model);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xuất excel danh sách đăng ký kiểm tra
        /// </summary>
        /// <param name="year"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("{type}/export")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> ExportExcel (SearchCompleteModel model, int type)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await statisticsService.Export(model, type);
            return Ok(apiResultModel);
        }
    }
}
