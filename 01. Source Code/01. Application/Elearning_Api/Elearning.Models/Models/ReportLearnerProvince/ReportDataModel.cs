using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearnerProvince
{
    public class ReportDataModel
    {
        public string LearnerId { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? ProvinceId { get; set; }
        public string? DistrictId { get; set; }
        public string? WardId { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DateOfBirthday { get; set; }
    }
}
