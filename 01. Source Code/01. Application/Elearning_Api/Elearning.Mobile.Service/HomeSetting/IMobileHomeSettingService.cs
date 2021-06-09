using Elearning.Model.Models.Mobile.HomeSetting;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.HomeSetting
{
    public interface IMobileHomeSettingService
    {
        Task<HomeSettingMobileModel> GetHomeSetting ();
    }
}
