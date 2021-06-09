using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.NumberSubscribersStatistics
{
    public class ResultNumberSubscribersModel
    {
        public string ProgramId { get; set; }
        public string CourseName { get; set; }
        public string CourseId { get; set; }
        public int NumberSubscribers { get; set; }
        public DateTime? RegistrationDate { get; set; }
    }
}
