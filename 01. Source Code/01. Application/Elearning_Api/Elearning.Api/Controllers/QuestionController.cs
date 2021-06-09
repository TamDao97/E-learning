using Elearning.Api.Attributes;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Question;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Questions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/questions")]
    [ApiController]
    [ValidateModel]
    [NTSAuthorize]
    public class QuestionController : BaseApiController
    {
        private readonly IQuestionService questionService;

        public QuestionController(IQuestionService questionService)
        {
            this.questionService = questionService;
        }

        /// <summary>
        /// Tìm kiếm câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<QuestionSearchResultModel>>> Search([FromBody] QuestionsSearchConditionModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await questionService.SearchAsync(searchModel, GetLevelRequest(), GetManageUnitRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tạo câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> Create([FromBody] QuestionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await questionService.CreateAsync(Request,model, userId, GetManageUnitRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhập câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        public async Task<ActionResult<ApiResultModel>> Update([FromBody] QuestionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await questionService.UpdateAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Chi tiết câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> GetQuestionById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await questionService.GetQuestionByIdAsync(id);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> Delete([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await questionService.DeleteAsync(Request, id, userId);

            return Ok(apiResultModel);
        }

        //[HttpGet]
        //public ActionResult Get()
        //{

        //    string path = AppDomain.CurrentDomain.BaseDirectory + "template.html";
        //    FileStream filestreampath = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        //    using (WordDocument document = new WordDocument(filestreampath, FormatType.Html, XHTMLValidationType.Transitional))
        //    {
        //        string pathEx = Path.Combine(Directory.GetCurrentDirectory(), "result.docx");
        //        MemoryStream stream = new MemoryStream();
        //        document.Save(stream, FormatType.Docx);
        //        document.Close();
        //        stream.Position = 0;
        //        //Process.Start("result.docx");
        //        return File(stream, "application/msword", "HTMLtoWord.docx");
        //    }

        //    //string fileHtml = AppDomain.CurrentDomain.BaseDirectory + "sample.html";
        //    //string pathexport = Path.Combine(Directory.GetCurrentDirectory(), "result.docx");
        //    //FileStream fileStreamPath = new FileStream(fileHtml, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        //    //using (WordDocument document = new WordDocument(fileStreamPath, FormatType.Html))
        //    //{
        //    //    MemoryStream stream = new MemoryStream();
        //    //    document.Save(stream, FormatType.Docx);
        //    //    document.Close();
        //    //    stream.Position = 0;
        //    //    pathexport =
        //    //}

        //    //return File(stream, "application/msword", "Result.docx");
        //    //Process.Start("HTMLtoWord.docx");

        //    return Ok();
        //}

        [HttpPut]
        [Route("update-status/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateStatusQuestion ([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await questionService.UpdateStatusQuestionsync(Request, id, userId);
            return Ok(apiResultModel);
        }

        /// <summary>
        /// Yêu cầu duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("request/{id}")]
        public async Task<ActionResult<ApiResultModel>> RequestQuestion([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await questionService.RequestQuestionAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Duyệt, Không duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approval/{id}")]
        public async Task<ActionResult<ApiResultModel>> ApprovalQuestion([FromRoute] string id, [FromBody] StatusModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await questionService.ApprovalQuestionAsync(id, userId, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Danh sách lịch sử bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("approval-history/{id}")]
        public async Task<ActionResult<ApiResultModel>> GetListQuestionApprovalStatus([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await questionService.GetListQuestionApprovalStatusAsync(id);

            return Ok(apiResultModel);
        }
    }
}
