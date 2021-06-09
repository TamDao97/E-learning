using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.CompleteStatistics
{
    public class ResultCompleteExportModel
    {
        public int Index { get; set; }
        public string CourseName { get; set; }
        public int TotalComplete { get; set; }
        public int TotalIncomplete { get; set; }
    }
}
