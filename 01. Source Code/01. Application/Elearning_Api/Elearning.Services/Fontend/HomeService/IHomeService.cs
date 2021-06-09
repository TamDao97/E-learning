using Elearning.Model.Models.Fontend.HomeService;
using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.HomeService
{
    public interface IHomeService
    {
        Task<SearchBaseResultModel<HomeServiceFrontEndModel>> SearchHomeServiceAsync();
        Task<List<HomeLinkFrontendModel>> GetAllHomeLinkAsync ();
    }
}
