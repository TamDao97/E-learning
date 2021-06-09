using System;
using System.Collections.Generic;
using System.Text;

namespace NTS.Common.Users
{
    public class NtsUserLoginModel
    {
        public string UserName { get; set; }
        public string Name { get; set; }
        public string ImageLink { get; set; }
        public string UserId { get; set; }
        public int Type { get; set; }
        public string EmployeeId { get; set; }
        public bool IsLogin { get; set; }
        public bool IsDisable { get; set; }
        public string Password { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public string Secret { get; set; }
        public int ExpireDateAfter { get; set; }
        public string DeviceId { get; set; }
        public string ManagerUnitId { get; set; }

        public int ManagerUnitLevel { get; set; }

        public List<string> Permissions { get; set; }

        public NtsUserLoginModel ()
        {
            Permissions = new List<string>();
        }
    }
}
