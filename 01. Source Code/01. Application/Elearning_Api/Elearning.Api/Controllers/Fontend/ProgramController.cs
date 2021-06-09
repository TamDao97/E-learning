using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Fontend.Client_Program;
using Elearning.Model.Models.Fontend.Course;
using Elearning.Models;
using Elearning.Services.Fontend.Program;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Frontend
{
    [Route("api/fontend/program")]
    [ApiController]
    public class ProgramController : BaseApiController
    {
        private readonly IProgramService program;
        public ProgramController(IProgramService program)
        {
            this.program = program;
        }

        [HttpPost]
        [Route("search/{id}")]
        public async Task<ActionResult> GetListProgram(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await program.GetListProgram(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Load ra danh sách chương trình và khóa học của chương trình đó
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("search")]
        public async Task<ActionResult> GetProgram(string learnerId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await program.GetProgramAsync(learnerId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy 2 khóa học có nhiều người học nhất
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("gettop2Course")]
        public async Task<ActionResult> GetTop2Course()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await program.GetTop2Course();

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy chi tiết chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("searchProgram")]
        public async Task<ActionResult<ApiResultModel>> GetProgramById(SearchProgram searchProgram)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await program.GetProgramByIdAsync(searchProgram);

            return Ok(apiResultModel);
        }
        [HttpPost]
        [Route("create-learner-course")]
        public async Task<ActionResult<ApiResultModel>> CreateLearnerCourse ([FromBody] LearnerCourseCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await program.CreateLearnerCourseAsync(Request, model);

            return Ok(apiResultModel);
        }
    }
}
