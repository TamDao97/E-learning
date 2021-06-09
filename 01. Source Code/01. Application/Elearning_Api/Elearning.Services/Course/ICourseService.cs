using Elearning.Model.Models.ApprovalHistory;
using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Fontend.Exam;
using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;
using Elearning.Models.Base;
using Elearning.Models.Combobox;
using Elearning.Models.User.Employee;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Course
{
    public interface ICourseService
    {
        /// <summary>
        /// Tìm kiếm hướng dẫn viên
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<MentorResultModel>> SearchMentor(string id);

        /// <summary>
        /// Tìm kiếm khóa học
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseStatusModel<CourseResultModel>> SearchCourseAsync(CourseSearchModel searchModel, int level, string managerUnitId);

        /// <summary>
        /// Xóa khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteCourseByIdAsync(HttpRequest request, string id, string userId);

        /// <summary>
        /// Cập nhập trạng thái hiển thị
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateStatusCourseAsync(HttpRequest request, string id, string userId);
        Task CreateCourseAsync(HttpRequest request, CourseCreateModel model, string userId, string manageUnit);
        Task UpdateCourseAsync(HttpRequest request, string id, CourseCreateModel model, string userId);
        Task<CourseInfoModel> GetCourseByIdAsync(string id);

        /// <summary>
        /// Lấy danh sách bài giảng 
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<LessonModel>> GetListLesson(LessonModelSearch modelSearch);


        /// <summary>
        /// Lấy bài giảng theo khóa học 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<LessonModel>> SearchLessonByCourseId(string courseId);

        /// <summary>
        /// Lấy người học theo khóa học 
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<LearnerResultModel>> SearchLearner(string courseId);
        Task<SearchBaseResultModel<ProgressModel>> GetProgress(ProgressSearchModel model);

        /// <summary>
        /// Danh sách  khóa học theo HDV
        /// </summary>
        /// <returns></returns>
        Task<List<EmployeeCourseModel>> GetEmployeeCourseAsync(string programId, string employeeId);

        Task<List<TestResultModel>> GetTestResult(string courseId, string learnerId);
        Task<List<FileTemplateModel>> GetFileTemplates();
        Task<string> PrintCertificate(CertificateModel model);
        Task<List<CbbOrderStringModel>> GetListOrder();
        Task<List<QuestionModel>> GetQuestionByLessonId(string testId);

        /// <summary>
        /// Yêu cầu duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task RequestCourseAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task ApprovalCourseAsync(string id, string userId, StatusModel model);

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<ApprovalHistoryModel>> GetListApprovalStatusAsync(string id);

        /// <summary>
        /// Lấy bài thi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ExamModel> GetExamByIdAsync(string id);
    }
}
