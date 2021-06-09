using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ApprovalHistory
{
    public class ApprovalHistoryModel
    {
        public long Id { get; set; }
        public string CourseId { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        public DateTime ProcessingDate { get; set; }
        public int ApprovalStatus { get; set; }
    }
}
