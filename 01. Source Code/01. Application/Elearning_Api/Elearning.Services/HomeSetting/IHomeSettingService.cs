using Elearning.Model.Models.HomeSetting;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.HomeSetting
{
    public interface IHomeSettingService
    {
        Task<HomeSettingInfoModel> GetHomeSettingAsync ();
        /// <summary>
        /// Lấy thông tin thiết lập trang chủ theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<HomeSettingInfoModel> GetHomeSettingByIdAsync (int id, string userId);
        /// <summary>
        /// Thêm thiết lập trang chủ
        /// </summary>
        /// <param name="homeSettingModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateHomeSettingAsync (HttpRequest request, HomeSettingModel homeSettingModel, string userId);
        /// <summary>
        /// Cập nhập thiết lập trang chủ
        /// </summary>
        /// <param name="id"></param>
        /// <param name="homeSettingModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateHomeSettingAsync (HttpRequest request, int id, HomeSettingModel homeSettingModel, string userId);
    }
}
