using Elearning.Model.Models.ReportLearner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.ReportLearner
{
    public interface IReportLearnerService
    {
        /// <summary>
        /// Thống kê người học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ReportLearnerResultModel> ReportLearner(ReportLearnerSearchConditionModel model);

        /// <summary>
        /// Xuất excel
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ExportFileModel> ExportExcelAsync(ReportLearnerSearchConditionModel model);

        /// <summary>
        /// Xuất pdf
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<ExportFileModel> ExportPdfAsync(ReportLearnerSearchConditionModel model);
    }
}
