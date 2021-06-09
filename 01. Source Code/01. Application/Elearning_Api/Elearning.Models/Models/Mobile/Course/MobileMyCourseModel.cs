using Elearning.Model.Models.Mobile.Program;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.Course
{
    public class MobileMyCourseModel
    {
        /// <summary>
        /// Tổng số khóa học
        /// </summary>
        public int TotalCourse { get; set; }
        /// <summary>
        /// Số khóa học hoàn thành
        /// </summary>
        public string Completed { get; set; }
        /// <summary>
        /// Số bài test hoàn thành
        /// </summary>
        public string TestComplete { get; set; }
        /// <summary>
        /// Danh sách khóa học của tôi
        /// </summary>
        public List<MobileMyCourseInfoModel> Courses { get; set; }
    }
}
