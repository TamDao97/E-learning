using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.MobileCourse
{
    public class MobileEmployeeCourseModel
    {
        /// <summary>
        /// Id hướng dẫn viên khóa học
        /// </summary>
        public string EmployeeId { set; get; }

        /// <summary>
        /// Tên hướng dẫn viên khóa học
        /// </summary>
        public string EmployeeName { set; get; }

        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string ImagePath { set; get; }

        /// <summary>
        /// Tổng số khóa học theo người hướng dẫn
        /// </summary>
        public int TotalCourse { set; get; }
    }
}
