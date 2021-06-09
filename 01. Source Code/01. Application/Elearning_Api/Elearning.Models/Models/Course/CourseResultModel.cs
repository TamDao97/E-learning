using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
   public class CourseResultModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string ProgramName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ManageUnitId { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int DisplayIndex { get; set; }
        public int ApprovalStatus { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public string ManageUnit  { get; set; }
    }
}
