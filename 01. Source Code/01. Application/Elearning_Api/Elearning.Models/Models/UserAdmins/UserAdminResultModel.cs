using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    public class UserAdminResultModel
    {
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id nhân viên
        /// </summary>
        public string EmployeeId { get; set; }

        /// <summary>
        /// Tên tài khoản
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Tình trạng khóa
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Họ và tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? Birthday { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Hình ảnh
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Nơi công tác
        /// </summary>
        public string WorkUnit { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        public string ManagerUnitId { get; set; }
        public string ManagerUnitName { get; set; }
        public int ManagerUnitLevel { get; set; }

    }
}
