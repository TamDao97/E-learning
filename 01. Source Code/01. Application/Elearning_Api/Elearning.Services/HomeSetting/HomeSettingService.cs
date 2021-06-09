using Elearning.Model.Models.HomeSetting;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.UserDevice;
using Elearning.Services.Log;
using Microsoft.AspNetCore.Http;

namespace Elearning.Services.HomeSetting
{
    public class HomeSettingService : IHomeSettingService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IDetectionService _detection;


        public HomeSettingService (ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;

        }
        public async Task CreateHomeSettingAsync (HttpRequest request, HomeSettingModel homeSettingModel, string userId)
        {
            Elearning.Model.Entities.HomeSetting homeSetting = new Elearning.Model.Entities.HomeSetting()
            {
                Logo=homeSettingModel.Logo,
                Address=homeSettingModel.Address,
                Phone=homeSettingModel.Phone,
                Gmail=homeSettingModel.Gmail,
                Copyright=homeSettingModel.Copyright,
                LinkFacebook=homeSettingModel.LinkFacebook,
                LinkGoogle=homeSettingModel.LinkGoogle,
                LinkYoutube=homeSettingModel.LinkYoutube,
                CreateDate = DateTime.Now,
                Website=homeSettingModel.Website,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            _sqlContext.HomeSetting.Add(homeSetting);
            _sqlContext.SaveChanges();


            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới thiết lập thông tin: " + homeSettingModel.Gmail;
            LogService.Event(userHistory, _detection);
        }

        public async Task<HomeSettingInfoModel> GetHomeSettingAsync ()
        {
           var data = (from a in _sqlContext.HomeSetting.AsNoTracking()
                       select new HomeSettingInfoModel
                       {
                           Id = a.Id,
                           Logo = a.Logo,
                           Address = a.Address,
                           Phone = a.Phone,
                           Gmail = a.Gmail,
                           Website=a.Website,
                           Copyright = a.Copyright,
                           LinkFacebook = a.LinkFacebook,
                           LinkGoogle = a.LinkGoogle,
                           LinkYoutube = a.LinkYoutube,
                       }).FirstOrDefault();

            if (data == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSetting);
            }
            return data;
        }

        public async Task<HomeSettingInfoModel> GetHomeSettingByIdAsync (int id, string userId)
        {
            var topicInfo = (from a in _sqlContext.HomeSetting.AsNoTracking()
                             where a.Id.Equals(id)
                             select new HomeSettingInfoModel
                             {
                                 Id = a.Id,
                                 Logo = a.Logo,
                                 Address = a.Address,
                                 Phone = a.Phone,
                                 Gmail = a.Gmail,
                                 Website=a.Website,
                                 Copyright = a.Copyright,
                                 LinkFacebook = a.LinkFacebook,
                                 LinkGoogle = a.LinkGoogle,
                                 LinkYoutube = a.LinkYoutube,
                             }).FirstOrDefault();

            if (topicInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSetting);
            }
            return topicInfo;
        }
        public async Task UpdateHomeSettingAsync (HttpRequest request, int id, HomeSettingModel homeSettingModel, string userId)
        {
            var homeSetting = await _sqlContext.HomeSetting.FindAsync(id);
            string NameOld = homeSetting.Gmail;
            if (homeSetting == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSetting);
            }

            homeSetting.Logo = homeSettingModel.Logo;
            homeSetting.Address = homeSettingModel.Address;
            homeSetting.Phone = homeSettingModel.Phone;
            homeSetting.Gmail = homeSettingModel.Gmail;
            homeSetting.Website = homeSettingModel.Website;
            homeSetting.Copyright = homeSettingModel.Copyright;
            homeSetting.LinkFacebook = homeSettingModel.LinkFacebook;
            homeSetting.LinkGoogle = homeSettingModel.LinkGoogle;
            homeSetting.LinkYoutube = homeSettingModel.LinkYoutube;
            homeSetting.UpdateBy = userId;
            homeSetting.UpdateDate = DateTime.Now;
            _sqlContext.SaveChanges();


            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == homeSetting.Gmail.ToLower())
            {
                userHistory.Content = "Cập nhật giao diện trang chủ: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật giao diện trang chủ: " + NameOld + " thành " + homeSetting.Gmail;
            }
            LogService.Event(userHistory, _detection);
        }
    }
}
