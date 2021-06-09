using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class CourseCreateModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Code, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
        [Display(Name = TextResourceKey.Description, ResourceType = typeof(TextResource))]
        [MaxLength(500, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public List<string> EmployeeCourses { get; set; }
        public List<string> LearnerCourses { get; set; }
        public List<LessonCourseModel> LessonCourses { get; set; }
        public bool IsDelete { get; set; }
        public int DisplayIndex { get; set; }
        public string CertificateTemplateId { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public string RequestBy { get; set; }
        public string ManagerUnitId { get; set; }
    }
}
