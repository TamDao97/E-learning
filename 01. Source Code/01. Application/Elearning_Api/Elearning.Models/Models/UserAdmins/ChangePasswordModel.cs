using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    /// <summary>
    /// Model thay đổi mật khẩu
    /// </summary>
    public class ChangePasswordModel
    {
        /// <summary>
        /// Mật khẩu
        /// </summary>
        public string PasswordHash { get; set; }
        public bool isPassword { get; set; }
    }
}
