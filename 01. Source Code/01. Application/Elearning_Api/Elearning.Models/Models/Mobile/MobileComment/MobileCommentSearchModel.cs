using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileComment
{
    public class MobileCommentSearchModel : SearchBaseModel
    {
        /// <summary>
        /// id khóa học
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// id bài giảng
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// id đăng nhập
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Loại
        /// 1: Khóa học
        /// 2: Bài giảng
        /// </summary>
        public int Type { get; set; }
    }
}
