using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Models.User.Employee
{
    public class EmployeeCreateModel
    {
        /// <summary>
        /// Id
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Name, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
        /// <summary>
        /// Mã nhân viên
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Code, ResourceType = typeof(TextResource))]
        [MaxLength(50, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Code { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime BirthDay { get; set; }
        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Email, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Email { get; set; }
        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public bool Gender { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Người dùng
        /// </summary>
        public UserModel UserModel { get; set; }
    }
}
