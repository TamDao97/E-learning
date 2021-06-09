using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
   public partial class Learner
    {
        public Learner ()
        {
            LearnerCourse = new HashSet<LearnerCourse>();
            LessonHistory = new HashSet<LessonHistory>();
            Mark = new HashSet<Mark>();
        }
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime? DateOfBirthday { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public int? NationId { get; set; }
        public bool? Gender { get; set; }
        public string Avatar { get; set; }
        public string Email { get; set; }
        public bool IsDisable { get; set; }
        public string Address { get; set; }
        public string IdToken { get; set; }
        public string Token { get; set; }
        public string Provider { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public Nullable<bool> IsLogin { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual ICollection<LearnerCourse> LearnerCourse { get; set; }
        public virtual ICollection<LessonHistory> LessonHistory { get; set; }
        public virtual ICollection<Mark> Mark { get; set; }
    }
}
