using System;
using System.Collections.Generic;
using System.Text;
using Elearning.Models.Base;

namespace Elearning.Models.UserCustomer
{
    public class UserAdminSearch:SearchBaseModel
    {
        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string WorkUnit { get; set; }

        /// <summary>
        /// Số điện thoại người dùng
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email người dùng
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Trạng thái khóa
        /// </summary>
        public bool? IsDisable { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Đơn vị quản lý
        /// </summary>
        public string ManagerUnitId { get; set; }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public int Type { get; set; }
    }
}
