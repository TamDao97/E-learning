using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.NumberSubscribersStatistics
{
    public class SearchNumberSubscribersModel : SearchBaseModel
    {
        public string ProgramId { get; set; }
        public string TimeType { get; set; }
        public int Month { get; set; }
        public int Year { get; set; }
        public int Quarter { get; set; }
    }
}
