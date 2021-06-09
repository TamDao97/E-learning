using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class LearnerCourse
    {
        public LearnerCourse()
        {
        }

        [Key]
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string LearnerId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? CertificateDatePrint { get; set; }
        public bool AllowedExam { get; set; }
        public bool AllowedCertificate { get; set; }
        public virtual Course Course { get; set; }
        public virtual Learner Learner { get; set; }
    }
}
