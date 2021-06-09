using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.History
{
    public class HistorySearchModel: SearchBaseModel
    {
        public string Content { get; set; }
        public int Type { get; set; }
        public string UserId { get; set; }
    }
}
