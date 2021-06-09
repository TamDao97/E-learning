using Elearning.Model.Models.ReportLearnerProvince;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.ReportLearnerProvince
{
    public interface IReportLearnerProvinceService
    {
        /// <summary>
        /// Thống kê thông tin người học
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        Task<ReportLearnerProvinceResultModel> ReportLearnerProvince(SearchReportLearnerModel model);

        /// <summary>
        /// Xuất file
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<string> ExportFileAsync(ReportLearnerProvinceModel model);
    }
}
