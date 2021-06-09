using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class LessonHistoryModel
    {
        public long Id { get; set; }
        public string LessonId { get; set; }
        public string LearnerId { get; set; }
        public string CourseId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
    }
}
