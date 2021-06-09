using Elearning.Model.Models.SystemParam;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elearning.Services.SystemParams
{
    public interface ISystemParamService
    {
        /// <summary>
        /// Lấy danh sách thông số hệ thống
        /// </summary>
        /// <returns></returns>
        Task<List<SystemParamModel>> GetListSystemParamAsync();

        /// <summary>
        /// Cập nhật thông số hệ thống
        /// </summary>
        /// <returns></returns>
        Task UpdateSystemParamAsync(List<SystemParamModel> listSystemParam);
    }
}
