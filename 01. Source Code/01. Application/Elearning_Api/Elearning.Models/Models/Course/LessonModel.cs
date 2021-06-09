using Elearning.Model.Models.LessonFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{

    /// <summary>
    /// Model chọn bài giảng cho khóa học
    /// </summary>
    public class LessonModel
    {
        public string Id { get; set; }

        /// <summary>
        /// Id bài giảng
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Tên bài giảng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mô tả bài giảng
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ảnh bài giảng
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Số thứ tự
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayIndex { get; set; }

        /// <summary>
        /// Danh mục
        /// </summary>
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }

        public int Type { get; set; }
        public int Time { get; set; }
        public List<LessonFrameModel> ListLessonFrame { get; set; }
        public LessonModel()
        {
            ListLessonFrame = new List<LessonFrameModel>();
        }
    }
}
