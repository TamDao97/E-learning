using Elearning.Model.Models.Fontend.HomeService;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Elearning.Services.Fontend.HomeService
{
    public class HomeService : IHomeService
    {

        private readonly ElearningContext sqlContext;

        public HomeService(ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        public async Task<List<HomeLinkFrontendModel>> GetAllHomeLinkAsync ()
        {
            var data = (from a in sqlContext.HomeLink.AsNoTracking()
                        where a.Status == true
                        select new HomeLinkFrontendModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            PageLink=a.PageLink
                        }).AsQueryable();

           
            return data.ToList();
        }

        public async Task<SearchBaseResultModel<HomeServiceFrontEndModel>> SearchHomeServiceAsync()
        {
            var data = (from a in sqlContext.HomeService.AsNoTracking()
                        where a.Status == true
                        orderby a.DisplayIndex
                        select new HomeServiceFrontEndModel
                        {
                            Id = a.Id,
                            Title = a.Title,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            DisplayIndex = a.DisplayIndex,
                        }).AsQueryable();

            SearchBaseResultModel<HomeServiceFrontEndModel> searchResult = new SearchBaseResultModel<HomeServiceFrontEndModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.Take(4).ToListAsync();

            return searchResult;
        }
    }
}
