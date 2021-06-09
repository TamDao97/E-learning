using Elearning.Model.Models.LessonAnswerLearner;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.LessonFrameHistory
{
    public class LessonFrameHistoryModel
    {
        public long Id { get; set; }
        public string LessonFrameId { get; set; }
        public string LessonId { get; set; }
        public string LearnerId { get; set; }
        public string CourseId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalCorrect { get; set; }
        public bool Pass { get; set; }
        public List<LessonAnswerLearnerModel> ListLessonAnswerLearner { get; set; }
        public LessonFrameHistoryModel()
        {
            ListLessonAnswerLearner = new List<LessonAnswerLearnerModel>();
        }
    }
}
