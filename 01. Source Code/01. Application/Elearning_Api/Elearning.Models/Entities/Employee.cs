using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elearning.Models.Entities
{
    public partial class Employee
    {
        [Key]
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
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
