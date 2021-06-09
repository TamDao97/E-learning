using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.MobileCourse
{
    public class MobileCourseDetailModel
    {
        /// <summary>
        /// Id khóa học
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Nội dung khóa học
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Tiêu đề khóa học
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mô tả khóa học
        /// </summary>
        public string Description { get; set; }

        public bool isRegister { get; set; }
    }
}
