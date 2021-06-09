using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Comment
{
    public class CommentSearchConditionModel : SearchBaseModel
    {
        /// <summary>
        /// Tình trạng
        /// </summary>
        public int? Status { get; set; }
        /// <summary>
        /// id khóa học
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// id bài giảng
        /// </summary>
        public string LessonId { get; set; }
        public string LessonFrameId { get; set; }
        /// <summary>
        /// id đăng nhập
        /// </summary>
        public string UserId { get; set; }
        public string Content { get; set; }
    }
}
