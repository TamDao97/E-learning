using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Answer
{
    public class AnswerInfoModel
    {
        public string Id { get; set; }
        public string AnswerLearnerId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerContentQuestion { get; set; }
        public string AnswerLabel { get; set; }
        public bool IsCorrect { get; set; }
        public bool IsCorrectQuestion { get; set; }
        public int DisplayIndex { get; set; }
        public int DisplayIndexQuestion { get; set; }
        public string Checked { get; set; }
        public int Type { get; set; }
    }
}
