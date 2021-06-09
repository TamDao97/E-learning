using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    public class UserFontEndResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirthday { get; set; }
        public string ProvinceId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictId { get; set; }
        public string DistrictName { get; set; }
        public string WardId { get; set; }
        public bool? IsDisable { get; set; }
        public bool? Gender { get; set; }
        public string WardName { get; set; }
        public int? NationId { get; set; }
        public string NationName { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Provider { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
