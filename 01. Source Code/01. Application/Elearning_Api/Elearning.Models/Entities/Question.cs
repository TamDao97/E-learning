using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class Question
    {
        public Question()
        {
            LessonQuestion = new HashSet<LessonQuestion>();
        }

        public string Id { get; set; }
        public string TopicId { get; set; }
        public string Content { get; set; }
        public string ContentClear { get; set; }
        public string ManagerUnitId { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual Topic Topic { get; set; }
        public virtual ICollection<LessonQuestion> LessonQuestion { get; set; }
    }
}
