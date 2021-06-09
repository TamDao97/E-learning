using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Elearning.Models.UserAdmin;

namespace Elearning.Models
{
    public class UserAdminModel
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
        /// Trạng thái tài khoản
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Nhóm quyền
        /// </summary>
        public string GroupUserId { get; set; }

        /// <summary>
        /// Loại tài khoản
        /// </summary>
        public int Type { get; set; }


        /// <summary>
        /// Họ và tên nhân viên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public bool Gender { get; set; }

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
        /// Link ảnh
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
        public int ManagerUnitLevel { get; set; }

        public List<GroupFunctionUserAdminModel> ListGroupFunction { get; set; }

    }
}
