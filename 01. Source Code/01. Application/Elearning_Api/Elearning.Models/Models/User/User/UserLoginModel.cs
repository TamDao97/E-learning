using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.User.User
{
    public class UserLoginModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirthday { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardId { get; set; }
        public string WardName { get; set; }
        public int? NationId { get; set; }
        public string NationName { get; set; }
        public Picture picture { get; set; }
        public string Avatar { get; set; }
        public bool? Gender { get; set; }
        public bool IsDisable { get; set; }
        public string Email { get; set; }
        public string IdToken { get; set; }
        public string access_token { get; set; }
        public string Provider { get; set; }
        public string Address { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public bool? IsLogin { get; set; }
        public List<CourseLear> ListCourse { get; set; }

        public UserLoginModel()
        {
            ListCourse = new List<CourseLear>();
        }
    }

    public class CourseLear
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }

    public class GoogleUserModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string picture { get; set; }
        public string Email { get; set; }

    }


    public class Picture
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public bool IsSilhouette { get; set; }
        public string Url { get; set; }
    }
}
