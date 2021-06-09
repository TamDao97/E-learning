using Elearning.Model.Models.Course;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EducationProgram
{
    public class ProgramInfoModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public bool IsChecked { get; set; }
        public int CheckCount { get; set; }
        public List<CourseInfoModel> ListCourse { get; set; }

        public ProgramInfoModel()
        {
            ListCourse = new List<CourseInfoModel>();
        }
    }
}
