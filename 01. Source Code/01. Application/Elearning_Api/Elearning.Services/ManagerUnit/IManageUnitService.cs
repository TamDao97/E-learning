using Elearning.Model.Models.ManageUnit;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.ManagerUnit
{
   public interface IManageUnitService
    {
        Task<SearchBaseResultModel<ManageUnitResultModel>> SearchManageUnitAsync (ManageUnitSearchModel searchModel);
        Task<ManageUnitInfoModel> GetManageUnitByIdAsync (string id, string userId);
        Task CreateManageUnitAsync (HttpRequest request, ManageUnitModel manageUnitModel, string userId);
        Task UpdateManageUnitAsync (HttpRequest request, string id, ManageUnitModel manageUnitModel, string userId);
        Task DeleteManageUnitByIdAsync (HttpRequest request, string id, string userId);
    }
}
