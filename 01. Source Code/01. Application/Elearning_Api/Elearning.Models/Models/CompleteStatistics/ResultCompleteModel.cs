using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.CompleteStatistics
{
    public class ResultCompleteModel
    {
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public int TotalComplete { get; set; }
        public int TotalIncomplete { get; set; }
    }
}
