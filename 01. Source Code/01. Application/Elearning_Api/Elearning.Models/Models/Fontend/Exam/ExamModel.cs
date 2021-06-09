using Elearning.Model.Models.Question;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class ExamModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string QuestionId { get; set; }
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public string TestId { get; set; }
        public int Type { get; set; }
        public bool IsExam { get; set; }
        public int? ExamTime { get; set; }
        public bool IsTest { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public string CourseId { get; set; }
        public string Slug { get; set; }
        public int ApprovalStatus { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalCorrect { get; set; }
        public List<QuestionCreateModel> ListQuestion { get; set; }
        public ExamModel()
        {
            ListQuestion = new List<QuestionCreateModel>();
        }
    }
}
