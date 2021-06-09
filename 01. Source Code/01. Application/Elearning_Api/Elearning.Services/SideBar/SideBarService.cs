using Elearning.Model.Models.HomeSillder;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;
using Wangkanai.Detection.Services;
using Microsoft.AspNetCore.Http;
using Elearning.Services.UserDevice;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.Log;

namespace Elearning.Services.SideBar
{
    public class SideBarService : ISideBarService
    {
        private readonly ElearningContext sqlContext;
        private readonly IDetectionService _detection;


        public SideBarService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this._detection = _detection;
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm  silder bar
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<HomeSilderResultModel>> SearchHomeSliderAsync(HomeSilderSearchModel searchModel)
        {
            var data = (from a in sqlContext.HomeSlider.AsNoTracking()
                        orderby a.DisplayIndex
                        select new HomeSilderResultModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            DisplayIndex = a.DisplayIndex,
                            Status = a.Status
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Title))
            {
                data = data.Where(i => i.Title.ToUpper().Contains(searchModel.Title.ToUpper()));
            }


            if (searchModel.Status.HasValue)
            {
                data = data.Where(i => i.Status.Equals(searchModel.Status));
            }

            SearchBaseResultModel<HomeSilderResultModel> searchResult = new SearchBaseResultModel<HomeSilderResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.DisplayIndex).ToListAsync();

            return searchResult;
        }

        public async Task<SearchBaseResultModel<HomeSilderResultModel>> SearchSliderAsync()
        {
            var data = (from a in sqlContext.HomeSlider.AsNoTracking()
                        where a.Status == true
                        orderby a.DisplayIndex
                        select new HomeSilderResultModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            DisplayIndex = a.DisplayIndex,
                        }).AsQueryable();

            SearchBaseResultModel<HomeSilderResultModel> searchResult = new SearchBaseResultModel<HomeSilderResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.ToListAsync();

            return searchResult;
        }

        /// <summary>
        /// Thêm  silder bar
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateHomeSliderAsync(HttpRequest request, HomeSilderCreateModel model, string userId)
        {

            Elearning.Model.Entities.HomeSlider HomeSlider = new Elearning.Model.Entities.HomeSlider()
            {
                //Id = index + 1,
                Title = model.Title,
                Description = model.Description,
                ImagePath = model.ImagePath,
                DisplayIndex = model.DisplayIndex,
                Status = model.Status,
                CreateBy = userId,
                UpdateBy = userId,
                CreateDate = DateTime.Now,
                UpdateDate = DateTime.Now,
            };

            sqlContext.HomeSlider.Add(HomeSlider);
            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới slider bar: " + model.Title;
            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Cập nhật  silder bar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateHomeSliderAsync(HttpRequest request, int id, HomeSilderCreateModel model, string userId)
        {
            var homeSliderExist = await sqlContext.HomeSlider.FirstOrDefaultAsync(i => i.Id.Equals(id));
            string NameOld = homeSliderExist.Title;
            if (homeSliderExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSlider);
            }


            homeSliderExist.Title = model.Title;
            homeSliderExist.Description = model.Description;
            homeSliderExist.ImagePath = model.ImagePath;
            homeSliderExist.DisplayIndex = model.DisplayIndex;
            homeSliderExist.Status = model.Status;
            homeSliderExist.UpdateBy = userId;
            homeSliderExist.UpdateDate = DateTime.Now;

            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == model.Title.ToLower())
            {
                userHistory.Content = "Cập nhật slider bar tên là: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật slider bar có tên ban đầu là: " + NameOld + " thành " + model.Title ;
            }
            LogService.Event(userHistory, _detection);
        }

        public async Task UpdateDisplayIndexAsync(HttpRequest request, DisplayIndexModex models,  string userId)
        {
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);


            if (models.ListDisplayIndex.Count > 0)
            {
                foreach (var item in models.ListDisplayIndex)
                {
                    var homeSliderExist = await sqlContext.HomeSlider.FirstOrDefaultAsync(i => i.Id.Equals(item.Id));
                    var indexold = homeSliderExist.DisplayIndex;

                    if (homeSliderExist == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSlider);
                    }
                    homeSliderExist.DisplayIndex = item.DisplayIndex;
                    homeSliderExist.UpdateBy = userId;
                    homeSliderExist.UpdateDate = DateTime.Now;

                    if (homeSliderExist.DisplayIndex != item.DisplayIndex)
                    {
                        userHistory.Content = "Cập nhật thứ tự slider bar: " + homeSliderExist.Title + " từ " + indexold + " thành " + item.DisplayIndex;
                        LogService.Event(userHistory, _detection);
                    }
                }
            }

            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy thông tin  silder bar
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<HomeSilderModel> GetHomeSliderByIdAsync(int id)
        {
            var resultInfo = await (from a in sqlContext.HomeSlider.AsNoTracking()
                                    where a.Id.Equals(id)
                                    select new HomeSilderModel()
                                    {
                                        Id = a.Id,
                                        Title = a.Title,
                                        Description = a.Description,
                                        ImagePath = a.ImagePath,
                                        DisplayIndex = a.DisplayIndex,
                                        Status = a.Status,
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSlider);
            }

            return resultInfo;
        }

        /// <summary>
        /// Xóa  silder bar
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteHomeSliderByIdAsync(HttpRequest request, int id, string userId)
        {
            var homeSliderExist = await sqlContext.HomeSlider.FirstOrDefaultAsync(i => i.Id.Equals(id));
            string programName = homeSliderExist.Title;
            if (homeSliderExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.HomeSlider);
            }

            sqlContext.HomeSlider.Remove(homeSliderExist);
            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa slider bar: " + programName;
            LogService.Event(userHistory, _detection);
        }
    }
}
