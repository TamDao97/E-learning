using System;
using System.Collections.Generic;

namespace Elearning.Model.Models.Mobile.Course
{
    public class MobileMyCourseInfoModel
    {

        /// <summary>
        /// Id khóa học
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// Tên khóa học
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime CreateDate { get; set; }
        /// <summary>
        /// Ảnh
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Số comment
        /// </summary>
        public int CommentNumber { get; set; }
        /// <summary>
        /// Tên giảng viên
        /// </summary>
        public List<string> EmployeeNames { get; set; }
        /// <summary>
        /// Số bài hoàn thành
        /// </summary>
        public int Completed { get; set; }
        /// <summary>
        /// Tổng số bài học
        /// </summary>
        public int TotalUnits { get; set; }
        /// <summary>
        /// Phân trăm hoàn thành
        /// </summary>
        public float Percent { get; set; }
    }
}
