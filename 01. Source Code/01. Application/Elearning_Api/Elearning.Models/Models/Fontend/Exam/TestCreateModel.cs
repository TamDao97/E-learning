using Elearning.Model.Models.Question;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class TestCreateModel
    {
        public long Id { get; set; }
        public string CourseId { get; set; }
        public string TestId { get; set; }
        public string LearnerId { get; set; }
        public string LessonId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalCorrect { get; set; }
        public int Type { get; set; }
        public List<QuestionCreateModel> ListQuestion { get; set; }
        public TestCreateModel()
        {
            ListQuestion = new List<QuestionCreateModel>();
        }
    }
}
