using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class EmployeeModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool Gender { get; set; }
        public DateTime? Birthday { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Avatar { get; set; }
        public string WorkUnit { get; set; }
        public string Description { get; set; }
        public int NumberOfLeared { get; set; }
        public int NumberOfCourse { get; set; }
    }
}
