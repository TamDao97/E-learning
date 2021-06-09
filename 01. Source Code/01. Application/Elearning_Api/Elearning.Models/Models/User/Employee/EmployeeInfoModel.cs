using Elearning.Models.GroupUser;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.User.Employee
{
   public class EmployeeInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime? BirthDay { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public bool Gender { get; set; }
        public int Status { get; set; }
        public UserModel UserModel { get; set; }
        public List<UserPermissionModel> ListFunctionParameter { get; set; }
        public List<GroupFunctionModel> ListGroupFunction { get; set; }
        public EmployeeInfoModel ()
        {
            UserModel = new UserModel();
            ListGroupFunction = new List<GroupFunctionModel>();
            ListFunctionParameter = new List<UserPermissionModel>();
        }
    }
}
