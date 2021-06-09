using Elearning.Api.Attributes;
using Elearning.Model.Models.Comment;
using Elearning.Model.Models.Course;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Services.Authen;
using Elearning.Services.Comments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    [Route("api/comments")]
    [ApiController]
    [ValidateModel]
    //[NTSAuthorize]
    public class CommentController : BaseApiController
    {
        private readonly ICommentService commentService;
        private readonly IAuthenService authenService;
        public CommentController(ICommentService commentService, IAuthenService authenService)
        {
            this.commentService = commentService;
            this.authenService = authenService;
        }

        /// <summary>
        /// Tìm kiếm bình luận
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultCommentModel<CommentSearchResultModel>>> SearchComment([FromBody] CommentSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.SearchCommentAsync(modelSearch, GetEmployeeIdByRequset(), GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới bình luận
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateComment([FromBody] CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            //string accessToken = HttpContext.Request.Headers["Authorization"];
            //var userInfo = await authenService.GetUserInfoAsync(accessToken);
            string userId = GetUserIdByRequest();

            await commentService.CreateCommentAsync(Request, model, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateComment([FromRoute] long id, [FromBody] CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string accessToken = HttpContext.Request.Headers["Authorization"];
            var userInfo = await authenService.GetUserInfoAsync(accessToken);
            await commentService.UpdateCommentAsync(Request, id, model, userInfo);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin bình luận theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<CommentCreateModel>>> GetCommentByIdAsync([FromRoute] long id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await commentService.GetCommentByIdAsync(id, userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Xóa bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteComment([FromRoute] long id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await commentService.DeleteCommentByIdAsync(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Duyệt bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("approved")]
        public async Task<ActionResult<ApiResultModel>> ApprovedComment([FromBody] List<long> listComment)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await commentService.ApprovedComment(Request, listComment, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Ẩn bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("unapproved")]
        public async Task<ActionResult<ApiResultModel>> UnApprovedComment([FromBody] List<long> listComment)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await commentService.UnApprovedComment(Request, listComment, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy số bình luận mới
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("get-total-new")]
        public async Task<ActionResult<ActionResult<string>>> GetCommentTotalNew()
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.GetCommentTotalNewAsync(GetEmployeeIdByRequset());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm bình luận font-end
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-font-end")]
        public async Task<ActionResult<SearchBaseResultCommentModel<CommentSearchResultModel>>> SearchCommentFontEnd([FromBody] CommentSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.SearchCommentFontEndAsync(modelSearch, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm bình luận font-end
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-comment-course")]
        public async Task<ActionResult<SearchBaseResultCommentModel<CommentSearchResultModel>>> SearchCommentFontEndCourse([FromBody] CommentSearchConditionModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.SearchCommentFontEndCourseAsync(modelSearch, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin khóa học và danh sách bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("lesson-course/{id}")]
        public async Task<ActionResult<ActionResult<object>>> GetLessonCourse([FromRoute] string id, LessonCourse model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.GetLessonCourseAsync(Request,id, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin bình luận theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("lesson/{id}")]
        public async Task<ActionResult<ActionResult<CommentCreateModel>>> GetLessonById([FromRoute] string id, CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await commentService.GetLessonByIdAsync(id, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới bình luận font-end
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("font-end")]
        public async Task<ActionResult<ApiResultModel>> CreateCommentFontEnd([FromBody] CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            //string accessToken = HttpContext.Request.Headers["Authorization"];
            //var userInfo = await authenService.GetUserInfoAsync(accessToken);
            await commentService.CreateCommentFontEndAsync(Request, model);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy thông tin danh mục bài giảng con theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("lesson-frame/{id}")]
        public async Task<ActionResult<ActionResult<CommentCreateModel>>> GetLessonFrameById([FromRoute] string id, CommentCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await commentService.GetLessonFrameByIdAsync(id, model);

            return Ok(apiResultModel);
        }
    }
}
