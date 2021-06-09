using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class CourseSearchModel:SearchBaseModel
    {
        public string Name { get; set; }
        public string ProgramId { get; set; }
        public int? ApprovalStatus { get; set; }
        public string ManageUnitId { get; set; }
    }
}
