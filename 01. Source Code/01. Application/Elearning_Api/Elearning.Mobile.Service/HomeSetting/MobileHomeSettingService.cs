using Elearning.Model.Models.Mobile.HomeSetting;
using Elearning.Models.Entities;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Elearning.Models.Settings;

namespace Elearning.Mobile.Service.HomeSetting
{
    public class MobileHomeSettingService : IMobileHomeSettingService
    {
        private readonly ElearningContext sqlContext;
        private readonly AppSettingModel appSettingModel;
        public MobileHomeSettingService (ElearningContext sqlContext, IOptions<AppSettingModel> options)
        {
            this.sqlContext = sqlContext;
            this.appSettingModel = options.Value;
        }
        public async Task<HomeSettingMobileModel> GetHomeSetting ()
        {
            var homeSetting = (from a in sqlContext.HomeSetting.AsNoTracking()
                               select new HomeSettingMobileModel
                               {
                                   Logo= appSettingModel.ServerApiUrl+ a.Logo,
                                   Address =a.Address,
                                   CopyRight=a.Copyright,
                                   Gmail=a.Gmail,
                                   Website=a.Website,
                                   LinkFacebook=a.LinkFacebook,
                                   LinkGoogle=a.LinkGoogle,
                                   LinkYouTube=a.LinkYoutube,
                                   Phone=a.Phone
                               }).FirstOrDefault();
            return homeSetting;
        }
    }
}
