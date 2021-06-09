using System;
using System.Collections.Generic;
using System.Text;
using Elearning.Model.Models.Fontend.Course;

namespace Elearning.Model.Models.Fontend.Client_Program
{
    public class ProgramModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public string Slug { get; set; }
        public List<CourseModel> ListCourse { get; set; }

    }

    public class SearchProgram
    {
        public string Slug { get; set; }
        public string LearnerId { get; set; }
    }

    public class CourseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
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
        public int DateDiff { get; set; }
        public string Slug { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public CourseModel()
        {
            ListLesson = new List<LessonModel>();
        }

    }
}
