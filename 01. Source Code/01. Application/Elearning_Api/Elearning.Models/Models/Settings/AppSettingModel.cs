using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Settings
{
    public class AppSettingModel
    {
        public string Secret { get; set; }
        public int ExpireDateAfter { get; set; }
        public string ServerFileUrl { get; set; }
        public string KeyAuthorize { get; set; }
        public string ServerApiUrl { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
