using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearnerProvince
{
    public class ReportLearnerProvinceModel
    {
        public int Type { get; set; }
        public int ExportType { get; set; }
        public List<ReportProvinceModel> ListResult { get; set; }
        public ReportLearnerProvinceModel()
        {
            ListResult = new List<ReportProvinceModel>();
        }
    }
}
