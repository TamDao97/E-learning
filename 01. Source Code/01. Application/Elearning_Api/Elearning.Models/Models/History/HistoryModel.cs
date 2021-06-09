using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.History
{
    public class HistoryModel
    {
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        public string Content { get; set; }
        public DateTime CreateDate { get; set; }
        public string ClientIP { get; set; }
        public string OS { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserName { get; set; }
        public string Device { get; set; }
    }
}
