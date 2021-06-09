using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Comment
{
    public class CommentCreateModel
    {
        /// <summary>
        /// Id comment
        /// </summary>
        public long? Id { get; set; }
        /// <summary>
        /// Id comment cha
        /// </summary>
        public Nullable<long> ParentCommentId { get; set; }
        /// <summary>
        /// Khóa học
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// Bài giảng
        /// </summary>
        public string LessonId { get; set; }
        public string LearnerId { get; set; }
        public string LessonFrameId { get; set; }
        /// <summary>
        /// Đối tượng
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public int ObjectType { get; set; }
        /// <summary>
        /// Loại commet
        /// 1: Khóa học
        /// 2: Bài giảng
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Ngày comment
        /// </summary>
        public DateTime CommentDate { get; set; }
        /// <summary>
        /// Tình trạng
        /// </summary>
        public int Status { get; set; }
    }
}
