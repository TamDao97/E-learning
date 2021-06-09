using Elearning.Model.Models.Mobile.MobileTest;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.Test
{
    public interface IMobileTestService
    {
        /// <summary>
        /// Lưu tạm đáp án câu hỏi trả lời
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns></returns>
        Task CreateTest(MobileTestCreateModel model);

        /// <summary>
        /// kết thúc bài thi trắc nghiệm
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        Task<object> CreateListTest(MobileTestCreateListModel model);

        /// <summary>
        /// kết thúc bài thi trắc nghiệm bài giảng chi tiết
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        Task<object> CreateListTestLessonFrame(string id, MobileTestCreateListModel model);
    }
}
