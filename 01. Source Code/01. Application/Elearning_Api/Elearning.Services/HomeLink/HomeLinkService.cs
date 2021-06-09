using Elearning.Model.Models.HomeLink;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.HomeLink
{
    public class HomeLinkService : IHomeLinkService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IDetectionService _detection;
        public HomeLinkService (ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;
        }
        public async Task CreateHomeLinkAsync (HttpRequest request, HomeLinkModel homeLinkModel, string userId)
        {
            Elearning.Model.Entities.HomeLink homeLink = new Elearning.Model.Entities.HomeLink()
            {
                Title=homeLinkModel.Title,
                Description=homeLinkModel.Description,
                PageLink= homeLinkModel.PageLink,
                CreateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            _sqlContext.HomeLink.Add(homeLink);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới home link: " + homeLinkModel.Title;
            LogService.Event(userHistory, _detection);
        }

        public async Task DeleteHomeLinkByIdAsync (HttpRequest request, int id, string userId)
        {
            var homeLinkExist = _sqlContext.HomeLink.Find(id);
            if (homeLinkExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeLink);
            }
            _sqlContext.HomeLink.Remove(homeLinkExist);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa home link";
            LogService.Event(userHistory, _detection);
        }

        public async Task<HomeLinkInfoModel> GetHomeLinkByIdAsync (int id, string userId)
        {
            var homeLinkInfo = (from a in _sqlContext.HomeLink.AsNoTracking()
                               where a.Id.Equals(id)
                               select new HomeLinkInfoModel
                               {
                                   Id = a.Id,
                                  Title=a.Title,
                                   Description = a.Description,
                                   Status = a.Status,
                                   PageLink = a.PageLink,
                                   CreateDate=a.CreateDate,
                                   CreateBy=a.CreateBy,
                                   UpdateDate=a.UpdateDate,
                                   UpdateBy=a.UpdateBy,
                               }).FirstOrDefault();

            if (homeLinkInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeLink);
            }
            return homeLinkInfo;
        }

        public async Task<SearchBaseResultModel<HomeLinkResultModel>> SearchHomeLinkAsync (HomeLinkSearchModel searchModel)
        {
            var data = (from a in _sqlContext.HomeLink.AsNoTracking()
                        select new HomeLinkResultModel
                        {
                            Id = a.Id,
                            Description = a.Description,
                            Status = a.Status,
                            PageLink=a.PageLink,
                            Title=a.Title,
                            CreateBy=a.CreateBy,
                            CreateDate=a.CreateDate,
                            UpdateBy=a.UpdateBy,
                            UpdateDate=a.UpdateDate,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Title))
            {
                data = data.Where(r => r.Title.ToUpper().Contains(searchModel.Title.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                data = data.Where(r => r.Description.ToUpper().Contains(searchModel.Description.ToUpper()));
            }
            SearchBaseResultModel<HomeLinkResultModel> searchResult = new SearchBaseResultModel<HomeLinkResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.OrderBy(s => s.CreateDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return searchResult;
        }

        public async Task UpdateHomeLinkAsync (HttpRequest request, int id, HomeLinkModel homeLinkModel, string userId)
        {
            var homeLink = await _sqlContext.HomeLink.FindAsync(id);
            if (homeLink == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeLink);
            }

            string NameOld = homeLink.Title.NTSTrim();

            homeLink.Title = homeLinkModel.Title;
            homeLink.Description = homeLinkModel.Description;
            homeLink.PageLink = homeLinkModel.PageLink;
            homeLink.UpdateBy = userId;
            homeLink.UpdateDate = DateTime.Now;
            homeLink.Status = homeLinkModel.Status;
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == homeLinkModel.Title.ToLower())
            {
                userHistory.Content = "Cập nhật home link tạo tên là: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật home link tạo có tên ban đầu là: " + NameOld + " thành " + homeLinkModel.Title;
            }
            LogService.Event(userHistory, _detection);
        }

        public async Task UpdateStatusHomeLinkAsync (HttpRequest request, int id, string userId)
        {
            var homeLinkExist = _sqlContext.HomeLink.Find(id);
            var homeLinkExistName = _sqlContext.HomeLink.Find(id).Title;
            if (homeLinkExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            if (homeLinkExist.Status == true)
            {
                homeLinkExist.Status = false;
            }
            else
            {
                homeLinkExist.Status = true;
            }

            _sqlContext.SaveChanges();
            var statusName = this.getNameStatus(homeLinkExist.Status);

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái home link: " + homeLinkExistName + " thành " + statusName;
            LogService.Event(userHistory, _detection);
        }
        public string getNameStatus (bool status)
        {
            if (status == true)
            {
                return "Hiển thị";
            }
            else
                return "Không hiển thị";
        }
    }
}
