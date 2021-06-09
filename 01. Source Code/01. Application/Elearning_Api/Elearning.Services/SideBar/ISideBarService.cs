using Elearning.Model.Models.HomeSillder;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.SideBar
{
    public interface ISideBarService
    {
        Task<SearchBaseResultModel<HomeSilderResultModel>> SearchHomeSliderAsync(HomeSilderSearchModel searchModel);

        Task<SearchBaseResultModel<HomeSilderResultModel>> SearchSliderAsync();

        Task CreateHomeSliderAsync(HttpRequest request, HomeSilderCreateModel model, string userId);

        Task UpdateHomeSliderAsync(HttpRequest request, int id, HomeSilderCreateModel model, string userId);

        Task UpdateDisplayIndexAsync(HttpRequest request, DisplayIndexModex models, string userId);

        Task<HomeSilderModel> GetHomeSliderByIdAsync(int id);

        Task DeleteHomeSliderByIdAsync(HttpRequest request, int id, string userId);
    }
}
