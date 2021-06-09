using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class QuestionModel
    {
        public string QuestionId { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public bool? IsCorrect { get; set; }

        public List<AnswerModel> Answers { get; set; }
        public List<AnswerLearnerModel> AnswerLearners { get; set; }
    }

    public class AnswerLearnerModel
    {
        public string AnswerId { get; set; }
        public string AnswerLabel { get; set; }
        public bool? IsCorrect { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerLearnerContent { get; set; }
        public int? DisplayIndex { get; set; }
    }

    public class AnswerModel
    {
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerLabel { get; set; }
        public bool? IsCorrect { get; set; }
        public int DisplayIndex { get; set; }
        public bool? IsChecked { get; set; }
    }
}
