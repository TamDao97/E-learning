using Elearning.Model.Entities;
using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class LessonCourse
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string CourseId { get; set; }
        public int DisplayIndex { get; set; }

        //public virtual Course Course { get; set; }
        public virtual Lesson Lesson { get; set; }
    }
}
