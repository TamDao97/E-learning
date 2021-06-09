using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class Course
    {
        public Course ()
        {
            LearnerCourse = new HashSet<LearnerCourse>();
            EmployeeCourse = new HashSet<EmployeeCourse>();
        }
        [Key]
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string ManagerUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public string Slug { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int DisplayIndex { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public string CertificateTemplateId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual Program Program { get; set; }
        public virtual ICollection<LearnerCourse> LearnerCourse { get; set; }
        public virtual ICollection<EmployeeCourse> EmployeeCourse { get; set; }
        public virtual ICollection<LessonCourse> LessonCourse { get; set; } 
    }
}
