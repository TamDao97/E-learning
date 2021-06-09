using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Course
{
    public class LessonModel
    {
        public string Id { get; set; }
        public string LessonCourseId { get; set; }
        public string CategoryId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public bool IsExam { get; set; }
        public string Slug { get; set; }
        public int? ExamTime { get; set; }
        public int DisplayIndex { get; set; }
    }
}
