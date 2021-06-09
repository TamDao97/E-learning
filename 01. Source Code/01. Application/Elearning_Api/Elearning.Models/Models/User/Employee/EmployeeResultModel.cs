using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.User.Employee
{
    public class EmployeeResultModel
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
        public string UserName { get; set; }
        public string ManageUnit { get; set; }
        public string Logo { get; set; }
    }
}
