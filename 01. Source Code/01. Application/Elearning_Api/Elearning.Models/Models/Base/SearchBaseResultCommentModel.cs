using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Base
{
    public class SearchBaseResultCommentModel<T>
    {
        public int TotalItems { get; set; }
        public int TotalNew { get; set; }
        public int Pending { get; set; }
        public int Approved { get; set; }
        public int UnApproved { get; set; }
        public int Delete { get; set; }
        public List<T> DataResults { get; set; }
        public List<T> DataResultsAll { get; set; }
    }
}
