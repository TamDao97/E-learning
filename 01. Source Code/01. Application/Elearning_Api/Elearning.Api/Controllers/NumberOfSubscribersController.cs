using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.NumberSubscribersStatistics;
using Elearning.Models;
using Elearning.Services.NumberOfSubscribersStatistics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/number-subscribers")]
    [ApiController]
    public class NumberOfSubscribersController : BaseApiController
    {
        private readonly INumberOfSubscribersService numberOfSubscribers;
        public NumberOfSubscribersController (INumberOfSubscribersService numberOfSubscribers)
        {
            this.numberOfSubscribers = numberOfSubscribers;
        }

        [HttpPost]
        [Route("statistical")]
        public async Task<ActionResult> CompleteStatistic ([FromBody] SearchNumberSubscribersModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await numberOfSubscribers.StatisticalNumberSubscribers(model);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xuất excel danh sách đăng ký kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("{type}/export")]
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> ExportExcel ([FromBody] SearchNumberSubscribersModel model, [FromRoute] int type)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await numberOfSubscribers.Export(model, type);
            return Ok(apiResultModel);
        }
    }
}
