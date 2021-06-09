using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Fontend.HomeService;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Fontend.HomeService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Fontend
{
    [Route("api/home-services")]
    [ApiController]
    public class HomeServiceController : ControllerBase
    {

        private readonly IHomeService homeService;
        public HomeServiceController(IHomeService homeService)
        {
            this.homeService = homeService;
        }

        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<HomeServiceFrontEndModel>>> SearchHomeServiceAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await homeService.SearchHomeServiceAsync();

            return Ok(apiResultModel);
        }
        [HttpGet]
        [Route("getAllHomeLink")]
        public async Task<ActionResult<SearchBaseResultModel<HomeServiceFrontEndModel>>> GetAllHomeLinkAsync ()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await homeService.GetAllHomeLinkAsync();

            return Ok(apiResultModel);
        }
    }
}
