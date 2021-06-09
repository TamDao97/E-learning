using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    public class ChangePass
    {
        public string Id { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordOld { get; set; }
    }
}
