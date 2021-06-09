using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.Leson
{
    public class LessonMobileModel
    {
        public string LessonId { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public int TypeLesson { get; set; }
        public int? ExamTime { get; set; }
        public string Description { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public List<QuestionMobileModel> ListQuestion { get; set; }

        public bool IsFinish { get; set; }

        /// <summary>
        /// Thời gian làm bài còn lại của bài thi
        /// </summary>
        public double RemainingTime  { get; set; } 

        /// <summary>
        /// Tổng câu trả lời đúng
        /// </summary>
        public int TotalCorrect { get; set; }

        /// <summary>
        /// Tổng số câu hỏi
        /// </summary>
        public int TotalQuestion { get; set; }
        public string ImagePath { get; set; }
    }
}
