using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Course
{
    public class TestResultModel
    {
        public string LessonId { get; set; }
        public string LessonName { get; set; }
        public List<ResultModel> Results { get; set; }
        public bool IsChecked { get; set; }
        public int DisplayIndex { get; set; }
    }

    public class ResultModel
    {
        public string TestId { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public DateTime TestDate { get; set; }
    }
}
