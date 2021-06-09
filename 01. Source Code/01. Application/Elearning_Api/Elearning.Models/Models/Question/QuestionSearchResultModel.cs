using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Question
{
    public class QuestionSearchResultModel
    {
        public string Id { get; set; }
        public string TopicId { get; set; }
        public string TopicName { get; set; }
        public string Content { get; set; }
        public string ManagerUnitId { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public string CreateByName { get; set; }
        public DateTime CreateDate { get; set; }
        public int? StatusApproval { get; set; }
    }
}
