using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elearning.Api.Attributes;
using Elearning.Models;
using Elearning.Models.Combobox;
using Elearning.Services.Combobox;

namespace Elearning.Api.Controllers
{
    [Route("api/combobox")]
    [ApiController]
    public class ComboboxController : BaseApiController
    {
        private readonly IComboboxService comboboxService;

        public ComboboxController(IComboboxService comboboxService)
        {
            this.comboboxService = comboboxService;
        }

        [HttpGet]
        [Route("get-barcodetypes")]
        public async Task<ActionResult<ApiResultModel>> GetAllBarcodeType()
        {
            var rs = await comboboxService.GetAllBarcodeType();
            return Ok(rs);
        }

        [HttpGet]
        [Route("get-codeTypes")]
        public async Task<ActionResult<ApiResultModel>> GetCodes()
        {
            var rs = await comboboxService.GetAllCodeType();
            return Ok(rs);
        }

        [HttpGet]
        [Route("get-parkingTypes")]
        public async Task<ActionResult<ApiResultModel>> GetParkingTypes()
        {
            var rs = await comboboxService.GetAllParkingType();
            return Ok(rs);
        }

        [HttpGet]
        [Route("GetListGroupuser")]
        public ActionResult<ApiResultModel<List<ComboboxModel>>> GetListGroupuser()
        {
            ApiResultModel<List<ComboboxModel>> apiResultModel = new ApiResultModel<List<ComboboxModel>>
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = comboboxService.GetListGroupuser();
            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-category")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetCategory()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetCategoryAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-category-parent")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetCategoryFull()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetCategoryFullAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-program")]
        public async Task<ActionResult> GetProgram ()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetAllProgram();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-topic")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetTopic()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetTopicAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-topic-full")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetTopicFullAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetTopicFullAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-home-slider")]
        public async Task<ActionResult<List<ComboboxParentModel>>> SearchHomeSlider()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.SearchHomeSlider();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("employees")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetEmployeeAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetEmployeeAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("home-specialist")]
        public async Task<ActionResult<List<ComboboxParentModel>>> GetHomeSpecialistAsync()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetHomeSpecialistAsync();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-province")]
        public async Task<ActionResult<ComboboxModel>> GetListProvince()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetListProvince();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-nation")]
        public async Task<ActionResult<ComboboxModel>> GetListNation()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetListNation();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-district/{provinceId}")]
        public async Task<ActionResult<ComboboxModel>> GetListDistrictByProvinceId(string provinceId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetListDistrictByProvinceId(provinceId);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-ward/{districtId}")]
        public async Task<ActionResult<ComboboxModel>> GetListWardByDistrictId(string districtId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetListWardByDistrictId(districtId);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-user")]
        public async Task<ActionResult<ComboboxModel>> GetUser()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetUser();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("search-learner")]
        public async Task<ActionResult<ComboboxModel>> GetLearner()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetLearner();

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("get-list-manage-unit")]
        public async Task<ActionResult<ComboboxUnitModel>> GetListManageUnit()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await comboboxService.GetListManageUnits();

            return Ok(apiResultModel);
        }
    }
}