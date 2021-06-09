using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Dashboard
{
    public class DashboardCourseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ProgramName { get; set; }
        public double TotalLearner { get; set; }
        public int TotalLesson { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public DateTime StartDate { get; set; }
    }
}
