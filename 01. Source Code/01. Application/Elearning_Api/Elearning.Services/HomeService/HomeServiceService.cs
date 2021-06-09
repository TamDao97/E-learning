using Elearning.Model.Models.Combobox;
using Elearning.Model.Models.HomeService;
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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.HomeService
{
    public class HomeServiceService : IHomeServiceService
    {
        private readonly IDetectionService _detection;
        private readonly ElearningContext _sqlContext;

        public HomeServiceService (ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;

        }
        public async Task CreateHomeServiceAsync (HttpRequest request, HomeServiceModel homeServiceModel, string userId)
        {
            var indexs = _sqlContext.HomeService.ToList();
            var maxIndex = 2;
            if (indexs.Count > 0)
            {
                maxIndex = indexs.Select(a => a.DisplayIndex).Max();
            }

            if (homeServiceModel.DisplayIndex <= maxIndex)
            {
                int modelIndex = homeServiceModel.DisplayIndex;
                var listOrder = _sqlContext.HomeService.AsNoTracking().Where(b => b.DisplayIndex >= modelIndex).ToList();
                if (listOrder.Count() > 0 && listOrder != null)
                {
                    foreach (var item in listOrder)
                    {
                        var updateOrder = _sqlContext.HomeService.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                        updateOrder.DisplayIndex++;
                    }
                }
            }

            Elearning.Model.Entities.HomeService homeService = new Elearning.Model.Entities.HomeService()
            {
                Description = homeServiceModel.Description,
                ImagePath = homeServiceModel.ImagePath,
                Status = homeServiceModel.Status,
                Title = homeServiceModel.Title,
                DisplayIndex = homeServiceModel.DisplayIndex,
                CreateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };

            _sqlContext.HomeService.Add(homeService);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới lời tựa: " + homeServiceModel.Title;
            LogService.Event(userHistory, _detection);
        }

        public async Task DeleteHomeServiceByIdAsync (HttpRequest request, int id, string userId)
        {
            var maxIndex = _sqlContext.HomeService.AsNoTracking().Select(a => a.DisplayIndex).Max();
            var homeServiceExist = await _sqlContext.HomeService.FindAsync(id);
            string homeServiceExistName = homeServiceExist.Title;
            if (homeServiceExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeService);
            }

            if (homeServiceExist.DisplayIndex <= maxIndex)
            {
                int modelIndex = homeServiceExist.DisplayIndex;
                var listHomeService = _sqlContext.HomeService.AsNoTracking().Where(b => b.DisplayIndex >= modelIndex).ToList();
                if (listHomeService.Count() > 0 && listHomeService != null)
                {
                    foreach (var item in listHomeService)
                    {
                        if (!item.Id.Equals(homeServiceExist.Id))
                        {
                            var updateUnit = _sqlContext.HomeService.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();

                            updateUnit.DisplayIndex--;
                        }

                    }
                }
            }

            _sqlContext.HomeService.Remove(homeServiceExist);

            await _sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa lời tựa: " + homeServiceExistName;
            LogService.Event(userHistory, _detection);
        }

        public async Task<HomeServiceInfoModel> GetHomeServiceByIdAsync (int id, string userId)
        {
            HomeServiceInfoModel homeService = await(from a in _sqlContext.HomeService.AsNoTracking()
                                                 where a.Id.Equals(id)
                                                 select new HomeServiceInfoModel()
                                                 {
                                                     Id = a.Id,
                                                     Description = a.Description,
                                                     DisplayIndex=a.DisplayIndex,
                                                     ImagePath = a.ImagePath,
                                                     Status = a.Status,
                                                     Title = a.Title,
                                                 }).FirstOrDefaultAsync();

            if (homeService == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeService);
            }
            return homeService;
        }

        public async Task<SearchBaseResultModel<HomeServiceResultModel>> SearchHomeServiceAsync (HomeServiceSearchModel searchModel)
        {
            var data = (from a in _sqlContext.HomeService.AsNoTracking()
                        select new HomeServiceResultModel
                        {
                            Id = a.Id,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            Status = a.Status,
                            Title = a.Title,
                            DisplayIndex=a.DisplayIndex,

                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Title))
            {
                data = data.Where(r => r.Title.ToUpper().Contains(searchModel.Title.ToUpper()));
            }
            if (searchModel.Status.HasValue)
            {
                data = data.Where(r => r.Status==searchModel.Status);
            }

            SearchBaseResultModel<HomeServiceResultModel> searchResult = new SearchBaseResultModel<HomeServiceResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Count();
            //sắp xếp, phân trang
            searchResult.DataResults = await data.OrderBy(s=>s.DisplayIndex).ToListAsync();
            return searchResult;
        }

        public async Task UpdateHomeServiceAsync (HttpRequest request, int id, HomeServiceModel homeServiceModel, string userId)
        {
            var homeService = await _sqlContext.HomeService.FindAsync(id);
            if (homeService == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeService);
            }
           
            var checkOrder = _sqlContext.HomeService.Where(b => b.DisplayIndex == homeServiceModel.DisplayIndex).FirstOrDefault();

            string NameOld = string.Empty;
            if (checkOrder != null)
            {
                var newOrder = _sqlContext.HomeService.Where(r => r.Id.Equals(homeServiceModel.Id)).FirstOrDefault();
                NameOld = newOrder.Title;
                int oldOrder = newOrder.DisplayIndex;
                if (checkOrder.DisplayIndex < newOrder.DisplayIndex)
                {
                    var listHomeServiceChange = _sqlContext.HomeService.Where(a => a.DisplayIndex > checkOrder.DisplayIndex && a.DisplayIndex < newOrder.DisplayIndex);
                    if (listHomeServiceChange.Count() > 0)
                    {
                        foreach (var item in listHomeServiceChange)
                        {
                            item.DisplayIndex++;
                        }

                    }
                    checkOrder.DisplayIndex++;
                }

                if (checkOrder.DisplayIndex > newOrder.DisplayIndex)
                {
                    var listOrderChange = _sqlContext.HomeService.Where(a => a.DisplayIndex > newOrder.DisplayIndex && a.DisplayIndex < checkOrder.DisplayIndex);
                    if (listOrderChange.ToList().Count() > 0)
                    {
                        foreach (var item in listOrderChange)
                        {
                            item.DisplayIndex--;
                        }
                    }
                    checkOrder.DisplayIndex = checkOrder.DisplayIndex - 1;
                }
                newOrder.Description = homeServiceModel.Description;
                newOrder.ImagePath = homeServiceModel.ImagePath;
                newOrder.Status = homeServiceModel.Status;
                newOrder.Title = homeServiceModel.Title;
                newOrder.DisplayIndex = homeServiceModel.DisplayIndex;
                newOrder.UpdateDate = DateTime.Now;
                newOrder.UpdateBy = userId;
            }
            else
            {
                var newOrder = _sqlContext.HomeService.Where(r => r.Id.Equals(homeServiceModel.Id)).FirstOrDefault();
                NameOld = newOrder.Title;

                var listOrder = (from a in _sqlContext.HomeService.AsNoTracking()
                                 orderby a.DisplayIndex ascending
                                 select new Elearning.Model.Entities.HomeService
                                 {
                                     Id = a.Id,
                                     Description = a.Description,
                                     ImagePath = a.ImagePath,
                                     DisplayIndex=a.DisplayIndex,
                                     Status = a.Status,
                                     Title = a.Title,
                                 }).AsQueryable();
                if (newOrder.DisplayIndex == 1 && listOrder.Count() == 1 && !homeServiceModel.DisplayIndex.Equals("1"))
                {
                    throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                }
                newOrder.Description = homeServiceModel.Description;
                newOrder.ImagePath = homeServiceModel.ImagePath;
                newOrder.Status = homeServiceModel.Status;
                newOrder.Title = homeServiceModel.Title;
                newOrder.DisplayIndex = homeServiceModel.DisplayIndex;
                newOrder.UpdateDate = DateTime.Now;
                newOrder.UpdateBy = userId;
            }

            await _sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == homeServiceModel.Title.ToLower())
            {
                userHistory.Content = "Cập nhật lời tựa: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật lời tựa tên ban đầu là: " + NameOld + " thành " + homeServiceModel.Title;
            }
            LogService.Event(userHistory, _detection);
        }
        public async Task<List<CbbOrderModel>> GetListOrder ()
        {
            List<CbbOrderModel> searchResult = new List<CbbOrderModel>();
            try
            {
                var ListModel = (from a in _sqlContext.HomeService.AsNoTracking()
                                 orderby a.DisplayIndex
                                 select new CbbOrderModel()
                                 {
                                     Id = a.Id,
                                     Order = a.DisplayIndex,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    CbbOrderModel addFirstIndex = new CbbOrderModel();
                    addFirstIndex.Order = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = _sqlContext.HomeService.AsNoTracking().Select(b => b.DisplayIndex).Max();
                    CbbOrderModel addIndex = new CbbOrderModel();
                    addIndex.Order = (maxIndex + 1);
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }
        public async Task UpdateStatusHomeServiceAsync (HttpRequest request, int id, string userId)
        {
            var homServiceExist = _sqlContext.HomeService.Find(id);
            if (homServiceExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeService);
            }
            if (homServiceExist.Status == true)
            {
                homServiceExist.Status = false;
            }
            else
            {
                homServiceExist.Status = true;
            }
            _sqlContext.SaveChanges();

            var statusName = this.getNameStatus(homServiceExist.Status);

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái lời tựa: " + homServiceExist.Title + " thành " + statusName;
            LogService.Event(userHistory, _detection);
        }

        public async Task UpdateIndexHomeServiceAsync (HttpRequest request, List<HomeServiceIndex> model, string userId)
        {
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            foreach (var item in model)
            {
                var Exist = _sqlContext.HomeService.Find(item.Id);
                var indexold = Exist.DisplayIndex;
                if(Exist==null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeService);
                }
                Exist.DisplayIndex = item.DisplayIndex;

                if(Exist.DisplayIndex != item.DisplayIndex)
                {
                    userHistory.Content = "Cập nhật thứ tự lời tựa: " + Exist.Title + " từ " + indexold + " thành " + item.DisplayIndex;
                    LogService.Event(userHistory, _detection);
                }

            }
            _sqlContext.SaveChanges();


        }

        public string getNameStatus(bool status)
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
