using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.ReportLearnerProvince
{
    public class ReportYearOldModel
    {
        public List<YearOldModel> ListYearOld { get; set; }
        public ReportYearOldModel()
        {
            ListYearOld = new List<YearOldModel>
            {
                new YearOldModel { Name = "Từ 18 đến 24 tuổi", Min = 18, Max = 24 },
                new YearOldModel { Name = "Từ 25 đến 34 tuổi", Min = 25, Max = 34 },
                new YearOldModel { Name = "Từ 35 đến 44 tuổi", Min = 35, Max = 44 },
                new YearOldModel { Name = "Từ 45 đến 54 tuổi", Min = 45, Max = 54 },
                new YearOldModel { Name = "Từ 55 đến 64 tuổi", Min = 55, Max = 64 },
                new YearOldModel { Name = "Từ 65 tuổi trở nên", Min = 65, Max = 200 },
            };
        }
    }

    public class YearOldModel
    {
        public string Name { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
    }
}
