using Elearning.Model.Models.Question;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class ExamTimeModel
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string LearnerId { get; set; }
        public string LessonId { get; set; }
        public int Type { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }

        public List<QuestionCreateModel> ListQuestion { get; set; }
        public ExamTimeModel()
        {
            ListQuestion = new List<QuestionCreateModel>();
        }

    }
}
