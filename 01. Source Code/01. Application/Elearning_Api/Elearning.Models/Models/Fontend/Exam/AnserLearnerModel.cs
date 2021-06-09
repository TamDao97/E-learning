using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class AnserLearnerModel
    {
        public string Id { get; set; }
        public int? DisplayIndex { get; set; }
        public int? LearnerDisplayIndex { get; set; }
        public string AnswerContent { get; set; }
        public string LearnerAnswerContent { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? LearnerIsCorrect { get; set; }
    }
}
