using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearner
{
    public class ReportLearnerSearchConditionModel : SearchBaseModel
    {
        public string TimeType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
        public string Code { get; set; }

        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
    }
}
