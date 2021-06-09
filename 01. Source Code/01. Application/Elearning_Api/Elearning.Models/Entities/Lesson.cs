using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class Lesson
    {
        public Lesson()
        {
            LessonCourse = new HashSet<LessonCourse>();
            LessonQuestion = new HashSet<LessonQuestion>();
        }

        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public bool IsExam { get; set; }
        public int? ExamTime { get; set; }
        public string ManagerUnitId { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string Slug { get; set; }
        public int TotalRequestCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<LessonCourse> LessonCourse { get; set; }
        public virtual ICollection<LessonQuestion> LessonQuestion { get; set; }
    }
}
