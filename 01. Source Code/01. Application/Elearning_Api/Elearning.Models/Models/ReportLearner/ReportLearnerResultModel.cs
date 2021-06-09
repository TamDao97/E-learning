using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearner
{
    public class ReportLearnerResultModel
    {
        public List<ReportLearnerModel> ListResult { get; set; }
        public List<string> ListLable { get; set; }
        public List<double> ListData { get; set; }
        public ReportLearnerResultModel()
        {
            ListResult = new List<ReportLearnerModel>();
            ListLable = new List<string>();
            ListData = new List<double>();
        }
    }
}
