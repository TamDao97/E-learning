using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class ProgressSearchModel : SearchBaseModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
    }
}
