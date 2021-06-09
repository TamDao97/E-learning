using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Question;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Questions
{
    public interface IQuestionService
    {
        /// <summary>
        /// Tạo mới câu hỏi
        /// </summary>
        /// <returns></returns>
        Task CreateAsync(HttpRequest request, QuestionCreateModel model, string userId, string manageUnit);

        /// <summary>
        /// Cập nhập câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task UpdateAsync(HttpRequest request, QuestionCreateModel model, string userId);

        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteAsync(HttpRequest request, string id, string userId);

        Task<QuestionCreateModel> GetQuestionByIdAsync(string id);

        Task<SearchBaseStatusModel<QuestionSearchResultModel>> SearchAsync(QuestionsSearchConditionModel modelSearch, int level, string managerUnitId);
        Task UpdateStatusQuestionsync (HttpRequest request, string id, string userId);

        /// <summary>
        /// Yêu cầu duyệt câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task RequestQuestionAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task ApprovalQuestionAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<QuestionApprovalHistoryModel>> GetListQuestionApprovalStatusAsync(string id);
    }
}
