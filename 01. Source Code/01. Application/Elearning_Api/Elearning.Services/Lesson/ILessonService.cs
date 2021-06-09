using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Lesson;
using Elearning.Model.Models.Question;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Lessons
{
    public interface ILessonService
    {
        /// <summary>
        /// Tìm kiếm bài giảng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseStatusModel<LessonSearchResultModel>> SearchLessonAsync(LessonSearchConditionModel searchModel, int level, string managerUnitId);

        /// <summary>
        /// Thêm bài giảng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateLessonAsync(HttpRequest request, LessonCreateModel model, string userId, string manageUnit);

        /// <summary>
        /// Cập nhật bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateLessonAsync(HttpRequest request, string id, LessonCreateModel model, string userId);

        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateStatus(HttpRequest request, string id, string userId);

        /// <summary>
        /// Lấy thông tin bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<LessonCreateModel> GetLessonByIdAsync(string id, string userId);

        /// <summary>
        /// Xóa bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteLessonByIdAsync(HttpRequest request, string id, string userId);

        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<List<LessonQuestionModel>> SaerchQuestion(QuestionSearchConditionModel searchModel);

        /// <summary>
        /// Danh sách câu hỏi random
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<LessonQuestionModel>> GetListQuestionRandom(QuestionRandomModel model);

        /// <summary>
        /// Yêu cầu duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RequestLessonAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ApprovalLessonAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<LessonApprovalHistoryModel>> GetListLessonApprovalStatusAsync(string id);
    }
}
