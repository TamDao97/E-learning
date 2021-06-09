using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Dashboard
{
    public class DashboardCourseTopModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public int TotalLearner { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
    }
}
