using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.MobileCourse
{
    public class MobileCourseResultModel
    {
        /// <summary>
        /// Id khóa học
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tiêu đề khóa học
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mô tả ngắn khóa học
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ảnh khóa học
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Ngày tạo khóa học
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Ngày kết thúc
        /// </summary>
        public DateTime FinishDate { get; set; }

        /// <summary>
        /// Danh sách hướng dẫn viên
        /// </summary>
        public List<string> ListEmployees { get; set; }

        /// <summary>
        /// Số lượng comment
        /// </summary>
        public int TotalComment { get; set; }
        /// <summary>
        /// Khóa học đã được đăng ký hay chưa
        /// </summary>
        public bool IsRegister { get; set; }

        public MobileCourseResultModel()
        {
            ListEmployees = new List<string>();
        }

    }
}
