using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Users
{
    public class UserTokenModel
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string Account { get; set; }
        public string DeviceId { get; set; }
        public string Address { get; set; }
        public string Token { get; set; }
        public string ImagePath { get; set; }
        public int ExpireDateAfter { get; set; }
        public IEnumerable<string> Permissions { get; set; }
    }
}
