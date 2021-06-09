using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileAnswer
{
    public class MobileAnswerLearnerModel
    {
        public string AnswerId { get; set; }
        public int DisplayIndex { get; set; }
        public string AnswerContent { get; set; }
        public bool? IsCorrect { get; set; }

        public int LearnerDisplayIndex { get; set; }
        public string LearnerAnswerContent { get; set; }
        public bool? LearnerIsCorrect { get; set; }

    }
}
