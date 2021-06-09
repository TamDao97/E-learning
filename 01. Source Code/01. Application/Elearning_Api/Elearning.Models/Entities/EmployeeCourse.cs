using Elearning.Models.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Elearning.Model.Entities
{
    public partial class EmployeeCourse
    {
        [Key]
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string EmployeeId { get; set; }
        public virtual Course Course { get; set; }
        public virtual Employee Employee { get; set; }
    }
}
