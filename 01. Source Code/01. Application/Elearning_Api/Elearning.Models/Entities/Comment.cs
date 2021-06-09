using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class Comment
    {
        public long Id { get; set; }
        public string CourseId { get; set; }
        public string LessonId { get; set; }
        public string LessonFrameId { get; set; }
        public Nullable<long> ParentCommentId { get; set; }
        public string ObjectId { get; set; }
        public int ObjectType { get; set; }
        public int? Type { get; set; }
        public string Content { get; set; }
        public DateTime CommentDate { get; set; }
        public int Status { get; set; }
        public virtual Course Course { get; set; }
        //public virtual Lesson Lesson { get; set; }
    }
}
