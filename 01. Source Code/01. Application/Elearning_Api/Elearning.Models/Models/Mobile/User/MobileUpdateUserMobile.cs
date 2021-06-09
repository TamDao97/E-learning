using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.User
{
    public class MobileUpdateUserMobile
    {
        /// <summary>
        /// Họ tên người học
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Số điện thoại
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirthday { get; set; }

        /// <summary>
        /// Tỉnh
        /// </summary>
        public string ProvinceId { get; set; }

        /// <summary>
        /// Huyện
        /// </summary>
        public string DistrictId { get; set; }

        /// <summary>
        /// Xã
        /// </summary>
        public string WardId { get; set; }

        /// <summary>
        /// Dân tộc
        /// </summary>
        public int? NationId { get; set; }

        /// <summary>
        /// Đường link ảnh
        /// </summary>
        public string Avatar { get; set; }

        /// <summary>
        /// Giới tính
        /// true: Nữ
        /// false: nam
        /// </summary>
        public bool? Gender { get; set; }

        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// địa chỉ
        /// </summary>
        public string Address { get; set; }
    }
}
