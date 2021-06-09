using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class LessonQuestion
    {
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string LessonId { get; set; }

        public virtual Lesson Lesson { get; set; }
        public virtual Question Question { get; set; }
    }
}
