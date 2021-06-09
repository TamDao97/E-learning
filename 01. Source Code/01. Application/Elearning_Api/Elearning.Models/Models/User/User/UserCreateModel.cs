using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.User.User
{
    public class UserCreateModel
    {
        /// <summary>
        /// Id nhân viên
        /// </summary>
        public string EmployeeId { get; set; }


        [Display(Name = "Tên tài khoản")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [MaxLength(100, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string UserName { get; set; }

        /// <summary>
        /// Trạng thái tài khoản
        /// </summary>
        [Display(Name = "Trạng thái tài khoản")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
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
        [Display(Name = "Họ và tên")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        [Display(Name = "Giới tính")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
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
        /// mật khẩu
        /// </summary>
        [Display(Name = "Password")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        public string Password { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        [Display(Name = "Email")]
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
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
    }
}
