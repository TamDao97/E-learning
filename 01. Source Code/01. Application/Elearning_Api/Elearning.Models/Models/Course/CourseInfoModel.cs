using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class CourseInfoModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsChecked { get; set; }
        //public int CheckCount { get; set; }
        public int DisplayIndex { get; set; }
        public string CertificateTemplateId { get; set; }
        public int ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public string RequestBy { get; set; }
        public string ManagerUnitId { get; set; }
    }
}
