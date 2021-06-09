using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class CourseSearchFrontendModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public string ProgramDescription { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int DisplayIndex { get; set; }
        public int NumberOfLesson { get; set; }
        public int NumberOfLearner { get; set; }
        public List<string> ListEmployeeName { get; set; }
        public DateTime StartDate { get; set; }
        public int NumberOfComment { get; set; }
        public int LessonCourseLearned { get; set; }
        public float PercentLearned { get; set; }
        public DateTime FinishDate { get; set; }
        public bool IsLearned { get; set; }
        public List<LessonModel> ListLesson { get; set; }
        public string Slug { get; set; }

        public CourseSearchFrontendModel ()
        {
            ListLesson = new List<LessonModel>();
        }
    }
}
