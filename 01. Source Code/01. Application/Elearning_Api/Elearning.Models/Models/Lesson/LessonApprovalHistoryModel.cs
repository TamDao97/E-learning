using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class LessonApprovalHistoryModel
    {
        public long Id { get; set; }
        public string LessonId { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        public DateTime ProcessingDate { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
