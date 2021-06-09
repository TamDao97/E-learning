using Elearning.Model.Models.HomeLink;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.HomeLink
{
   public interface IHomeLinkService
    {
        /// <summary>
        /// Tìm kiếm home link
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<HomeLinkResultModel>> SearchHomeLinkAsync (HomeLinkSearchModel searchModel);
        /// <summary>
        /// Lấy thông tin home link theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<HomeLinkInfoModel> GetHomeLinkByIdAsync (int id, string userId);
        /// <summary>
        /// Thêm home link
        /// </summary>
        /// <param name="HomeLinkModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateHomeLinkAsync (HttpRequest request, HomeLinkModel homeLinkModel, string userId);
        /// <summary>
        /// Cập nhập home link
        /// </summary>
        /// <param name="id"></param>
        /// <param name="HomeLinkModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateHomeLinkAsync (HttpRequest request, int id, HomeLinkModel homeLinkModel, string userId);
        /// <summary>
        /// Xóa home link
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteHomeLinkByIdAsync (HttpRequest request, int id, string userId);
        Task UpdateStatusHomeLinkAsync (HttpRequest request, int id, string userId);
    }
}
