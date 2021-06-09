using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.User.Employee
{
    public class EmployeeSearchConditionModel: SearchBaseModel
    {
        /// <summary>
        /// Tên nhân viên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public int? Status { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
    }
}
