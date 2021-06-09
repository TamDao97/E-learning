using Elearning.Model.Models.Comment;
using Elearning.Model.Models.Fontend.Exam;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.Exam
{
    public interface IExamService
    {
        /// <summary>
        /// Lấy bài thi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ExamModel> GetExamByIdAsync(string slug, CommentCreateModel model);

        /// <summary>
        /// Thêm bài kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> CreateTestAsync(HttpRequest request, TestCreateModel model, string userId);

        Task<SeachResultModel<ExamModel>> GetListQuestionAnswer(ExamQuestionModel model);
        Task CreateTest(SaveTempCreateModel model);
        Task<object> CreateListTest(FinishTestCreateModel model);
        Task<CourseFontendModel> GetLessonCourseAsync (HttpRequest request, string slug, string userId, List<LessonCoursesModel> listCol);
        Task<string> GetLessonIdByslug (string slug);

        /// <summary>
        /// Lấy bài thi trắc nghiệm danh mục
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ExamModel> GetExamFrameByIdAsync(string id, CommentCreateModel model);

        /// <summary>
        /// Kết thúc bài thi trắc nghiệm danh mục
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        Task<object> CreateListLessonFrame(string id, FinishTestCreateModel model);
    }
}
