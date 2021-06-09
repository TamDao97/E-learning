namespace Elearning.Model.Models.Course
{
    public class LessonCourseModel
    {
        public string Id { get; set; }
        public string LessonId { get; set; }
        public string CourseId { get; set; }
        public int DisplayIndex { get; set; }
    }
}
