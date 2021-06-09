using Elearning.Models.Base;
using Elearning.Model.Models.Comment;
using NTS.Common.Users;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elearning.Model.Models.Lesson;
using Microsoft.AspNetCore.Http;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.LessonFrame;

namespace Elearning.Services.Comments
{
    public interface ICommentService
    {
        /// <summary>
        /// Tìm kiếm bình luận
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultCommentModel<CommentSearchResultModel>> SearchCommentAsync(CommentSearchConditionModel searchModel, string employeeId, string userId);

        /// <summary>
        /// Thêm mới bình luận
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateCommentAsync(HttpRequest request, CommentCreateModel model, string userId);

        /// <summary>
        /// Cập nhật bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task UpdateCommentAsync(HttpRequest request, long id, CommentCreateModel model, NtsUserTokenModel user);

        /// <summary>
        /// Lấy thông tin bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CommentCreateModel> GetCommentByIdAsync(long id, string userId);

        /// <summary>
        /// Xóa bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteCommentByIdAsync(HttpRequest request, long id, string userId);

        /// <summary>
        /// Duyệt bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        Task ApprovedComment(HttpRequest request, List<long> listComment, string userId);

        /// <summary>
        /// Ẩn bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        Task UnApprovedComment(HttpRequest request, List<long> listComment, string userId);

        /// <summary>
        /// Lấy số bình luận mới
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<string> GetCommentTotalNewAsync(string employeeId);

        /// <summary>
        /// Tìm kiếm bình luận font-end
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<CommentSearchResultFontEndModel>> SearchCommentFontEndAsync(CommentSearchConditionModel searchModel, string userId);

        /// <summary>
        /// Tìm kiếm bình luận khóa học
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<CommentSearchResultFontEndModel>> SearchCommentFontEndCourseAsync(CommentSearchConditionModel searchModel, string userId);

        /// <summary>
        /// Lấy thông tin khóa học và danh sách bài giảng
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<object> GetLessonCourseAsync(HttpRequest request, string courseId, LessonCourse model);

        /// <summary>
        /// Lấy thông tin bài giảng theo lessonCourseId
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<LessonCreateModel> GetLessonByIdAsync(string id, CommentCreateModel model);

        /// <summary>
        /// Thêm mới bình luận font-end
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateCommentFontEndAsync(HttpRequest request, CommentCreateModel model);

        /// <summary>
        /// Lấy danh mục con bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LessonFrameModel> GetLessonFrameByIdAsync(string id, CommentCreateModel model);
    }
}
