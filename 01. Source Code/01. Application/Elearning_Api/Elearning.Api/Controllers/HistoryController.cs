using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.History;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.History;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers
{
    [Route("api/history")]
    [ApiController]
    [NTSAuthorize]
    public class HistoryController : BaseApiController
    {
        private readonly IHistoryService history;

        public HistoryController(IHistoryService history)
        {
            this.history = history;
        }

        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<HistoryModel>>> SearchHistory([FromBody] HistorySearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await history.SearchHistory(modelSearch);

            return Ok(apiResultModel);
        }

    }
}
