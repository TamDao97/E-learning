using Elearning.Model.Models.ManageUnit;
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
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.ManagerUnit
{
    public class ManageUnitService : IManageUnitService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IDetectionService _detection;
        public ManageUnitService (ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;
        }
        public async Task CreateManageUnitAsync (HttpRequest request, ManageUnitModel manageUnitModel, string userId)
        {
            Elearning.Model.Entities.ManagerUnit managerUnit = new Elearning.Model.Entities.ManagerUnit()
            {
                Id = Guid.NewGuid().ToString(),
               Name= manageUnitModel.Name,
               Logo=manageUnitModel.Logo,
               Level=2,
                CreateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            _sqlContext.ManagerUnit.Add(managerUnit);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới đơn vị chủ quản: " + manageUnitModel.Name;
            LogService.Event(userHistory, _detection);
        }

        public async Task DeleteManageUnitByIdAsync (HttpRequest request, string id, string userId)
        {
            if(id.Equals("1"))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0048, TextResourceKey.ManageUnit);
            }    
            var manageUnitExist = _sqlContext.ManagerUnit.Find(id);
            if (manageUnitExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ManageUnit);
            }
            _sqlContext.ManagerUnit.Remove(manageUnitExist);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa đơn vị chủ quản";
            LogService.Event(userHistory, _detection);
        }

        public async Task<ManageUnitInfoModel> GetManageUnitByIdAsync (string id, string userId)
        {
            var manageUnitInfo = (from a in _sqlContext.ManagerUnit.AsNoTracking()
                                where a.Id.Equals(id)
                                select new ManageUnitInfoModel
                                {
                                    Id = a.Id,
                                   Name=a.Name,
                                   Level=a.Level,
                                   Logo=a.Logo,
                                    CreateDate = a.CreateDate,
                                    CreateBy = a.CreateBy,
                                    UpdateDate = a.UpdateDate,
                                    UpdateBy = a.UpdateBy,
                                }).FirstOrDefault();

            if (manageUnitInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.ManageUnit);
            }
            return manageUnitInfo;
        }

        public async Task<SearchBaseResultModel<ManageUnitResultModel>> SearchManageUnitAsync (ManageUnitSearchModel searchModel)
        {
            var data = (from a in _sqlContext.ManagerUnit.AsNoTracking()
                        select new ManageUnitResultModel
                        {
                            Id = a.Id,
                            Name=a.Name,
                            Level=a.Level,
                            Logo=a.Logo
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            SearchBaseResultModel<ManageUnitResultModel> searchResult = new SearchBaseResultModel<ManageUnitResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.OrderBy(s => s.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return searchResult;
        }

        public async Task UpdateManageUnitAsync (HttpRequest request, string id, ManageUnitModel manageUnitModel, string userId)
        {
            var manageUnit = await _sqlContext.ManagerUnit.FindAsync(id);
            if (manageUnit == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeLink);
            }

            string NameOld = manageUnit.Name.NTSTrim();

            manageUnit.Name = manageUnitModel.Name;
            manageUnit.Logo = manageUnitModel.Logo;
            manageUnit.UpdateBy = userId;
            manageUnit.UpdateDate = DateTime.Now;
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == manageUnitModel.Name.ToLower())
            {
                userHistory.Content = "Cập nhật home link tạo tên là: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật home link tạo có tên ban đầu là: " + NameOld + " thành " + manageUnitModel.Name;
            }
            LogService.Event(userHistory, _detection);
        }
    }
}
