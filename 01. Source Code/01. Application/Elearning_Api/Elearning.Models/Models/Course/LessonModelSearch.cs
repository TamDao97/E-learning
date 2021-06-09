using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class LessonModelSearch
    {
        public List<string> ListIdSelect { get; set; }
        public string Name { get; set; }
        public string CategoryId { get; set; }

        public LessonModelSearch()
        {
            ListIdSelect = new List<string>();
        }
    }
}
