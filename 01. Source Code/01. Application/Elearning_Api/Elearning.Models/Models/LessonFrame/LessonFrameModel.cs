using Elearning.Model.Models.Lesson;
using Elearning.Model.Models.LessonFrameHistory;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.LessonFrame
{
    public class LessonFrameModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string EstimatedTime { get; set; }
        public int TestTime { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int? TotalRequestCorrect { get; set; }
        public int? TotalQuestion { get; set; }
        public int? DisplayIndex { get; set; }
        public bool Status { get; set; }
        public DateTime Estimated { get; set; }
        public List<LessonQuestionModel> ListQuestion { get; set; }
        public List<LessonFrameHistoryModel> ListLessonFrameHistory { get; set; }
        public LessonFrameModel()
        {
            ListQuestion = new List<LessonQuestionModel>();
            ListLessonFrameHistory = new List<LessonFrameHistoryModel>();
        }
    }
}
