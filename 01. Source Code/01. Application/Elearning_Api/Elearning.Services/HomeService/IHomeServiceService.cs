using Elearning.Model.Models.Combobox;
using Elearning.Model.Models.HomeService;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.HomeService
{
   public interface IHomeServiceService
    {
        /// <summary>
        /// Tìm kiếm lời tựa
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<HomeServiceResultModel>> SearchHomeServiceAsync (HomeServiceSearchModel searchModel);
        /// <summary>
        /// Lấy thông tin lời tựa theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<HomeServiceInfoModel> GetHomeServiceByIdAsync (int id, string userId);
        /// <summary>
        /// Thêm lời tựa
        /// </summary>
        /// <param name="homeServiceModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateHomeServiceAsync (HttpRequest request, HomeServiceModel homeServiceModel, string userId);
        /// <summary>
        /// Cập nhập lời tựa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="homeServiceModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateHomeServiceAsync (HttpRequest request, int id, HomeServiceModel homeServiceModel, string userId);
        /// <summary>
        /// Xóa lời tựa
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteHomeServiceByIdAsync (HttpRequest request, int id, string userId);
        Task<List<CbbOrderModel>> GetListOrder ();
        Task UpdateStatusHomeServiceAsync (HttpRequest request, int id, string userId);
        Task UpdateIndexHomeServiceAsync (HttpRequest request, List<HomeServiceIndex> model, string userId);
    }
}
