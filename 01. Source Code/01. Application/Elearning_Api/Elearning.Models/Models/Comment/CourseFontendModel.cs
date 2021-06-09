using Elearning.Model.Models.LessonFrame;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Comment
{
    public class CourseFontendModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Slug { get; set; }
        public List<LessonCoursesModel> ListLessonCourse { get; set; }
        public CourseFontendModel()
        {
            ListLessonCourse = new List<LessonCoursesModel>();
        }
    }

    public class LessonCoursesModel
    {
        public string Id { get; set; }
        public string CourseId { get; set; }
        public string LessonId { get; set; }
        public string LessonName { get; set; }
        public int Index { get; set; }
        public string ImagePath { get; set; }
        public int Type { get; set; }
        public bool IsExam { get; set; }
        public bool Status { get; set; }
        public string Slug { get; set; }
        public int Time { get; set; }
        public double Percent { get; set; }
        public bool Col { get; set; }
        public List<LessonFrameModel> ListLessonFrame { get; set; }
        public LessonCoursesModel()
        {
            ListLessonFrame = new List<LessonFrameModel>();
        }
    }
}
