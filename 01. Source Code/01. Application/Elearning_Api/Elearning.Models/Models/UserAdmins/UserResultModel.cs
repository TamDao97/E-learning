using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    public class UserResultModel
    {
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id nhân viên
        /// </summary>
        public string LearnerId { get; set; }

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
        /// Tên facebook
        /// </summary>
        public string FacebookName { get; set; }

        /// <summary>
        /// Tên google
        /// </summary>
        public string GoogleName { get; set; }
    }
}
