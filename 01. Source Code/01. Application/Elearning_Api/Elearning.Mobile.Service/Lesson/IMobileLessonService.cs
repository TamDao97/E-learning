using Elearning.Model.Models.Fontend.Course;
using Elearning.Model.Models.Mobile.Leson;
using Elearning.Model.Models.Mobile.MobileLesson;
using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.Lesson
{
    public interface IMobileLessonService
    {
        Task<List<MobileLessonModel>> SearchLessonByCourseId(string id, string userId);
		Task<LessonMobileModel> GetLessonByCourseId (string lessonId, string learnerId, string courseId);

        /// <summary>
        /// Lấy danh mục con bài giảng chi tiết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<LessonMobileModel> GetLessonFrameByIdAsync(string id, string learnerId, string courseId);

        Task CreateLessonHistory(LessonHistoryModel model);

        /// <summary>
        /// Lưu lịch sử học bài giảng chi tiết
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task CreateLessonFrameHistory(LessonFrameHistorysModel model);
    }
}
