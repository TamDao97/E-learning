using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.MobileComment
{
    public class MobileCommentModel
    {

        /// <summary>
        ///  Id khóa học
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// Id bài giảng
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Id người dùng đang đăng nhập.
        /// </summary>
        public string LearnerId { get; set; }

        /// <summary>
        /// Id hỏi đáp con
        /// </summary>
        public Nullable<long> ParentCommentId { get; set; }

        /// <summary>
        /// Loại commet
        /// 1: Khóa học
        /// 2: Bài giảng
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        public string Content { get; set; }

    }
}
