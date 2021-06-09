using System.Threading.Tasks;
using Elearning.Api.Attributes;
using Elearning.Model.Models.EducationProgram;
using Elearning.Model.Models.UserHistory;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Log;
using Elearning.Services.ProgramEducation;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Mvc;
using Wangkanai.Detection.Services;

namespace Elearning.Api.Controllers
{
    [Route("api/programs")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class ProgramController : BaseApiController
    {
        private readonly IProgramService program;
        private readonly IDetectionService _detection;

        public ProgramController (IProgramService program, IDetectionService _detection)
        {
            this.program = program;
            this._detection = _detection;
        }
        /// <summary>
        /// Tìm kiếm chủ đề
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<ProgramModel>>> SearchProgram ([FromBody] ProgramSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await program.SearchProgramAsync(modelSearch);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin chủ đề theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<ProgramModel>>> GetProgramById ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await program.GetProgramByIdAsync(id, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm chủ đề
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateProgram ([FromBody] ProgramModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await program.CreateProgramAsync(Request, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Cập nhập chủ đề
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateProgram ([FromRoute] string id, [FromBody] ProgramModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await program.UpdateProgramAsync(Request, id, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa chủ đề
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteProgram ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await program.DeleteProgramByIdAsync(Request ,id, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("update-status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusCourse ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await program.UpdateStatusProgramAsync(Request, id, userId);

            return Ok(apiResultModel);
        }
    }
}
