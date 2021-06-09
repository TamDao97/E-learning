using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class LearnerCourseCreateModel
    {
        public string LearnerId { get; set; }
        public string CourseId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
