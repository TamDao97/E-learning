using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Mobile.Service.Comment;
using Elearning.Model.Models.Comment;
using Elearning.Model.Models.Mobile.MobileComment;
using Elearning.Model.Models.MobileComment;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.Settings;
using Elearning.Services.Authen;
using Elearning.Services.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Elearning.Api.Controllers.Mobile
{
    [Route("api/mobile/comment")]
    [ApiController]
    public class MobileCommentController : BaseApiController
    {
        private readonly IMobileCommentService mobileComment;
        private readonly IAuthenService authenService;

        public MobileCommentController(IMobileCommentService mobileComment, IAuthenService authenService, ICommentService commentService)
        {
            this.mobileComment = mobileComment;
            this.authenService = authenService;

        }

        /// <summary>
        /// Danh sách phản hồi
        /// </summary>
        /// <param name="model">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<ApiResultModel>> SearchCommentFontEndAsync(MobileCommentSearchModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await mobileComment.SearchComment(model);

            return Ok(apiResultModel);
        }


        /// <summary>
        /// Thêm mới thông tin phản hồi theo bài giảng
        /// </summary>
        /// <param name="model">Dữ liệu thông tin phản hồi</param>
        /// <returns></returns>
        [HttpPost]
        [Route("create-comment")]
        public async Task<ActionResult<ApiResultModel>> CreateComment([FromBody] MobileCommentModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            //string accessToken = HttpContext.Request.Headers["Authorization"];
            //var userInfo = await authenService.GetUserInfoAsync(accessToken);
            await mobileComment.CreateComment(model);

            return Ok(apiResultModel);
        }

    }
}
