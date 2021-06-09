using System;

namespace Elearning.Model.Models.Course
{
    public class ProgressModel
    {
        public string CourseId { get; set; }
        public string LearnerId { get; set; }
        public string LessonId { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime RegistrationDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? LastDate { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Ward { get; set; }
        public int Hours { get; set; }
        public int AvailableCourse { get; set; }
        public int StudiedCourse { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalQuestion { get; set; }

    }
}
