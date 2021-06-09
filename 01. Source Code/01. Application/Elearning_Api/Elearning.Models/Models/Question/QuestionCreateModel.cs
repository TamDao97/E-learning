using Elearning.Model.Models.Answer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Question
{
    public class QuestionCreateModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string TopicId { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public bool? Status { get; set; }
        public int? ApprovalStatus { get; set; }
        public bool IsLessionQuession { get; set; }

        public List<AnswerInfoModel> ListAnswer { get; set; }
        public List<AnswerInfoModel> ListAnswerQuession { get; set; }

        public QuestionCreateModel()
        {
            ListAnswer = new List<AnswerInfoModel>();
            ListAnswerQuession = new List<AnswerInfoModel>();
        }
    }
}
