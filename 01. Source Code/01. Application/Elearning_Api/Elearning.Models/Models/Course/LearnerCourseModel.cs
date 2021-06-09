using System;

namespace Elearning.Model.Models.Course
{
    public class LearnerCourseModel
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string LearnerId { get; set; }
        public DateTime RegistrationDate { get; set; }
    }
}
