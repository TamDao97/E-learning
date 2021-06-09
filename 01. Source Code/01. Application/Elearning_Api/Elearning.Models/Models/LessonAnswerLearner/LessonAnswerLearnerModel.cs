using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.LessonAnswerLearner
{
    public class LessonAnswerLearnerModel
    {
        public long Id { get; set; }
        public long LessonFrameHistoryId { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public bool? IsCorrect { get; set; }
        public int? DisplayIndex { get; set; }
    }
}
