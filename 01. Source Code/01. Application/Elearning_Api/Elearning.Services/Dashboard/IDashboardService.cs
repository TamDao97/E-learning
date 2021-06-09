using Elearning.Model.Models.ReportLearner;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Dashboard
{
    public interface IDashboardService
    {
        /// <summary>
        /// Thông tin tổng hợp
        /// </summary>
        /// <returns></returns>
        Task<object> GetTotalAsync();
        
        /// <summary>
        /// Biểu đồ đăng ký khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> GetRegisterCourse(ReportLearnerSearchConditionModel model);
        
        /// <summary>
        /// Biểu đồ loại đăng ký
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> GetRegister(ReportLearnerSearchConditionModel model);
        
        /// <summary>
        /// Biểu đồ top tỉnh, thành phố
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<object> GetProvince(ReportLearnerSearchConditionModel model);
    }
}
