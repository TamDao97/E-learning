using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserHistory
{
    public class UserHistoryModel
    {
        public double Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string ClientIP { get; set; }
        public string OS { get; set; }
        public string Device { get; set; }
        public string BrowserVersion { get; set; }
        public string BrowserName { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }
    }
}
