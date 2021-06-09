using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearnerProvince
{
    public class ReportLearnerProvinceResultModel
    {
        public List<ReportProvinceModel> ReportProvinces { get; set; }
        public List<string> ReportProvincesLable { get; set; }
        public List<double> ReportProvincesData { get; set; }
        public List<ReportProvinceModel> ReportGender { get; set; }
        public List<string> ReportGenderLable { get; set; }
        public List<double> ReportGenderData { get; set; }
        public List<ReportProvinceModel> ReportYearOld { get; set; }
        public List<string> ReportYearOldLable { get; set; }
        public List<double> ReportYearOldData { get; set; }
        public ReportLearnerProvinceResultModel()
        {
            ReportProvinces = new List<ReportProvinceModel>();
            ReportProvincesLable = new List<string>();
            ReportProvincesData = new List<double>();
            ReportGender = new List<ReportProvinceModel>();
            ReportGenderLable = new List<string>();
            ReportGenderData = new List<double>();
            ReportYearOld = new List<ReportProvinceModel>();
            ReportYearOldLable = new List<string>();
            ReportYearOldData = new List<double>();
        }
    }
}
