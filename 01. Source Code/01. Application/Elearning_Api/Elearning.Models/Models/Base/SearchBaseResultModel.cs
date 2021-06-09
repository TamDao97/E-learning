using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Base
{
   public class SearchBaseResultModel<T>
    {
        public int TotalItems { get; set; }
        public List<T> DataResults { get; set; }
    }

    public class SeachResultModel<T>
    {
        public int TotalItems { get; set; }
        public double? TotalTime { get; set; }
        public bool isTime { get; set; }
        public DateTime? StateDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public List<T> DataResults { get; set; }
    }
}
