using Elearning.Model.Models.History;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using NTS.Common;

namespace Elearning.Services.History
{
    public class HistoryService : IHistoryService
    {
        private readonly ElearningContext _sqlContext;

        public HistoryService(ElearningContext _sqlContext)
        {
            this._sqlContext = _sqlContext;
        }

        public async Task<SearchBaseResultModel<HistoryModel>> SearchHistory(HistorySearchModel modelSearch)
        {
            SearchBaseResultModel<HistoryModel> resultModel = new SearchBaseResultModel<HistoryModel>();

            if (modelSearch.Type == 0)
            {

                var data = (from a in _sqlContext.UserHistories.AsNoTracking()
                            where a.Type.Equals(modelSearch.Type)
                            join b in _sqlContext.User.AsNoTracking() on a.UserId equals b.Id
                            join c in _sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
                            orderby a.CreateDate descending
                            select new HistoryModel
                            {
                                Content = a.Content,
                                BrowserName = a.BrowserName,
                                BrowserVersion = a.BrowserVersion,
                                ClientIP = a.ClientIP,
                                CreateDate = a.CreateDate,
                                Device = a.Device,
                                OS = a.OS,
                                UserName = b.UserName,
                                Name = c.Name,
                                UserId = b.Id,
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Content))
                {
                    data = data.Where(a => a.Content.ToLower().Contains(modelSearch.Content.ToLower()));
                }

                if (!string.IsNullOrEmpty(modelSearch.UserId))
                {
                    data = data.Where(a => a.UserId.Equals(modelSearch.UserId));
                }

                if (modelSearch.DateFrom.HasValue)
                {
                    data = data.Where(a => a.CreateDate >= DateTimeHelper.ToStartDate(modelSearch.DateFrom.Value));
                }

                if (modelSearch.DateTo.HasValue)
                {
                    data = data.Where(a => a.CreateDate <= DateTimeHelper.ToEndDate(modelSearch.DateTo.Value));
                }

                resultModel.TotalItems = data.Count();

                resultModel.DataResults = await data.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize)
                    .Take(modelSearch.PageSize)
                    .ToListAsync();
            }
            else if (modelSearch.Type == 1)
            {
                var data = (from a in _sqlContext.UserHistories.AsNoTracking()
                            where a.Type.Equals(modelSearch.Type)
                            join b in _sqlContext.Learner.AsNoTracking() on a.UserId equals b.Id
                            orderby a.CreateDate descending
                            select new HistoryModel
                            {
                                Content = a.Content,
                                BrowserName = a.BrowserName,
                                BrowserVersion = a.BrowserVersion,
                                ClientIP = a.ClientIP,
                                CreateDate = a.CreateDate,
                                Device = a.Device,
                                OS = a.OS,
                                UserName = b.Name,
                                UserId = b.Id,
                            }).AsQueryable();

                if (!string.IsNullOrEmpty(modelSearch.Content))
                {
                    data = data.Where(a => a.Content.ToLower().Contains(modelSearch.Content.ToLower()));
                }

                if (!string.IsNullOrEmpty(modelSearch.UserId))
                {
                    data = data.Where(a => a.UserId.Equals(modelSearch.UserId));
                }

                if (modelSearch.DateFrom.HasValue)
                {
                    data = data.Where(a => a.CreateDate >= DateTimeHelper.ToStartDate(modelSearch.DateFrom.Value));
                }

                if (modelSearch.DateTo.HasValue)
                {
                    data = data.Where(a => a.CreateDate <= DateTimeHelper.ToEndDate(modelSearch.DateTo.Value));
                }

                resultModel.TotalItems = data.Count();


                resultModel.DataResults = await data.Skip((modelSearch.PageNumber - 1) * modelSearch.PageSize)
                    .Take(modelSearch.PageSize)
                    .ToListAsync();

            }

            return resultModel;
        }
    }
}
