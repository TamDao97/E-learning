using Elearning.Model.Models.Fontend.Client_Program;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class CourseDetailModel
    {
        public string Id { get; set; }
        public string ProgramId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int TotalLearnerCourse { get; set; }
        public string Slug { get; set; }
        public List<LessonModel> ListLesson { get; set; }
        public List<EmployeeModel> ListEmployee { get; set; }
        public List<CourseModel> ListRelatedCourse { get; set; }

        public CourseDetailModel()
        {
            this.ListLesson = new List<LessonModel>();
            this.ListEmployee = new List<EmployeeModel>();
            this.ListRelatedCourse = new List<CourseModel>();
        }
    }
}
