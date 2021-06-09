using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.UserCustomer
{
    public class UserAdminInfoModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id tài khoản
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Mã người dùng
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên người dùng
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Địa chỉ
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Giới tính
        /// </summary>
        public string Gender { get; set; }

        /// <summary>
        /// Số chứng minh thư
        /// </summary>
        public string IdentifyNum { get; set; }

        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime DateOfIssue { get; set; }

        /// <summary>
        /// Nơi cấp
        /// </summary>
        public string PlaceOfIssue { get; set; }

        /// <summary>
        /// Số giấy phép đăng kí lái xe
        /// </summary>
        public string DriverLicenseNo { get; set; }

        /// <summary>
        /// Hạn giấy phép lái xe
        /// </summary>
        public string Expires { get; set; }

        /// <summary>
        /// Id hãng xe
        /// </summary>
        public string ClassificationId { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public string UpdateBy { get; set; }
    }
}
