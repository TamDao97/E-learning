using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class CertificateModel
    {
        public string TemplateId { get; set; }
        public string CourseName { get; set; }
        public string CourseId{ get; set; }
        public List<string> LearnerIds { get; set; }

        public CertificateModel()
        {
            LearnerIds = new List<string>();
        }
    }
}
