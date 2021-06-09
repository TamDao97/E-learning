using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.User.Learner
{
    /// <summary>
    /// Model thêm mới tài khoản người học
    /// </summary>
    public class UserLearnerCreateModel
    {
        /// <summary>
        /// Họ và tên người đăng ký.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Password người đăng ký.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Email người đăng ký.
        /// </summary>
        public string Email { get; set; }

    }
}
