using Elearning.Model.Models.EmployeeSpecialist;
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
using Elearning.Model.Entities;
using Microsoft.AspNetCore.Http;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.UserDevice;
using Elearning.Services.Log;
using Wangkanai.Detection.Services;

namespace Elearning.Services.EmployeeSpecialist
{
    public class EmployeeSpecialistService : IEmployeeSpecialistService
    {
        private readonly ElearningContext sqlContext;
        private readonly IDetectionService _detection;


        public EmployeeSpecialistService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this._detection = _detection;
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm chuyên gia
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<HomeSpecialistResultModel> SearchEmployeeSpecialistAsync()
        {
            var homespecialist = (from a in sqlContext.HomeSpecialist.AsNoTracking()
                                  select new HomeSpecialistResultModel()
                                  {
                                      Id = a.Id,
                                      Title = a.Title,
                                      Description = a.Description,
                                  }).FirstOrDefault();

            homespecialist.employeeSpecialists = (from a in sqlContext.EmployeeSpecialist.AsNoTracking()
                                                  join b in sqlContext.Employee.AsNoTracking() on a.EmployeeId equals b.Id
                                                  join c in sqlContext.HomeSpecialist.AsNoTracking() on a.HomeSpecialistId equals c.Id
                                                  orderby b.Name
                                                  select new EmployeeSpecialistResultModel
                                                  {
                                                      Id = b.Id,
                                                      Name = b.Name,
                                                      EmployeeId = b.Id,
                                                      Avartar = a.Avartar,
                                                      HomeSpecialistId = a.HomeSpecialistId,
                                                      Description = a.Description,
                                                      Title = c.Title,
                                                      Facebook =a.Facebook,
                                                      Lotus = a.Lotus,
                                                      Instagram = a.Instagram,
                                                      Twitter = a.Twitter
                                                  }).ToList();

            return homespecialist;
        }

        /// <summary>
        /// Thêm chuyên gia
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateEmployeeSpecialistAsync(HttpRequest request, EmployeeSpecialistCreateModel model, string userId)
        {
            var data = sqlContext.HomeSpecialist.Select(r => r.Id).ToList();
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (data.Count <= 0)
            {
                HomeSpecialist homeSpecialist = new HomeSpecialist()
                {
                    Title = model.Title,
                    Description = model.Description,
                };

                userHistory.Content = "Thêm mới cấu hình chuyên gia: " + model.Title;


                sqlContext.HomeSpecialist.Add(homeSpecialist);

                await sqlContext.SaveChangesAsync();

                LogService.Event(userHistory, _detection);


            }
            this.CreateEmployee(request, model, userId);

        }

        private async Task CreateEmployee(HttpRequest request, EmployeeSpecialistCreateModel model, string userId)
        {

            var id = sqlContext.HomeSpecialist.Select(r => r.Id).FirstOrDefault();

            List<Model.Entities.EmployeeSpecialist> employeeSpecialists = new List<Model.Entities.EmployeeSpecialist>();

            if (model.ListEmployeeSpeciallist.Count > 0)
            {
                foreach (var item in model.ListEmployeeSpeciallist)
                {
                    Elearning.Model.Entities.EmployeeSpecialist employee = new Model.Entities.EmployeeSpecialist()
                    {
                        EmployeeId = item.Id,
                        HomeSpecialistId = id,
                        Avartar = item.Avartar,
                        Description = item.Description,
                        Facebook = item.Facebook,
                        Lotus = item.Lotus,
                        Instagram = item.Instagram,
                        Twitter = item.Twitter,
                    };

                    employeeSpecialists.Add(employee);
                }

            }


            sqlContext.EmployeeSpecialist.AddRange(employeeSpecialists);
            await sqlContext.SaveChangesAsync();

        }

        /// <summary>
        /// Cập nhật chuyên gia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateEmployeeSpecialistAsync(HttpRequest request, int id, EmployeeSpecialistCreateModel model, string userId)
        {
            var data = sqlContext.HomeSpecialist.Where(a => a.Id.Equals(id)).FirstOrDefault();
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            data.Title = model.Title;
            data.Description = model.Description;

            List<Model.Entities.EmployeeSpecialist> employeeSpecialists = new List<Model.Entities.EmployeeSpecialist>();

            if (model.ListEmployeeSpeciallist.Count > 0)
            {
                var employees = sqlContext.EmployeeSpecialist.AsNoTracking().Where(a => a.HomeSpecialistId.Equals(id)).ToList();
                sqlContext.EmployeeSpecialist.RemoveRange(employees);

                foreach (var item in model.ListEmployeeSpeciallist)
                {
                    Elearning.Model.Entities.EmployeeSpecialist employee = new Model.Entities.EmployeeSpecialist()
                    {
                        EmployeeId = item.Id,
                        HomeSpecialistId = id,
                        Avartar = item.Avartar,
                        Description = item.Description,
                        Facebook = item.Facebook,
                        Lotus = item.Lotus,
                        Instagram = item.Instagram,
                        Twitter = item.Twitter,
                    };

                    employeeSpecialists.Add(employee);

                }
            }

            if (data.Title.ToLower() == model.Title.ToLower())
            {
                userHistory.Content = "Cập nhật cấu hình chuyên gia tên là: " + data.Title;
            }
            else
            {
                userHistory.Content = "Cập nhật cấu hình chuyên gia có tên ban đầu là: " + data.Title + " thành " + model.Title;
            }


            sqlContext.EmployeeSpecialist.AddRange(employeeSpecialists);
            await sqlContext.SaveChangesAsync();


            


            LogService.Event(userHistory, _detection);

        }

        /// <summary>
        /// Lấy thông tin chuyên gia
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<EmployeeSpecialistModel> GetEmployeeSpecialistByIdAsync(int id)
        {
            var resultInfo = await (from a in sqlContext.EmployeeSpecialist.AsNoTracking()
                                    where a.Id.Equals(id)
                                    select new EmployeeSpecialistModel()
                                    {
                                        Id = a.Id,
                                        Avatar = a.Avartar,
                                        Description = a.Description,
                                        EmployeeId = a.EmployeeId,
                                        HomeSpecialistId = a.HomeSpecialistId,
                                        
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeSpecialist);
            }

            return resultInfo;
        }

        /// <summary>
        /// Xóa chuyên gia
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteEmployeeSpecialistByIdAsync(HttpRequest request, int id, string userId)
        {
            var EmployeeSpecialistExist = await sqlContext.EmployeeSpecialist.FirstOrDefaultAsync(i => i.Id.Equals(id));
            var EmployeeSpecialistName = sqlContext.HomeSpecialist.Where(a => a.Id.Equals(EmployeeSpecialistExist.HomeSpecialistId)).FirstOrDefault();
            if (EmployeeSpecialistExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.EmployeeSpecialist);
            }
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa cấu hình chuyên gia: " + EmployeeSpecialistName.Title;
            sqlContext.EmployeeSpecialist.Remove(EmployeeSpecialistExist);
            await sqlContext.SaveChangesAsync();


            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Lấy danh sách chuyên viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<EmployeeSpecialResultModel>> SearchEmployee(EmployeeSearchModel searchModel)
        {
            var data = (from a in sqlContext.Employee.AsNoTracking()
                        join b in sqlContext.User.AsNoTracking() on a.Id equals b.ObjectId
                        where b.Type == 2 && !searchModel.ListIdSelect.Contains(a.Id) && b.IsDisable == false
                        orderby a.Name
                        select new EmployeeSpecialResultModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Avartar = a.Avatar,
                            Birthday = a.Birthday,
                            Email = a.Email,
                            Description = a.Description,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(i => i.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            SearchBaseResultModel<EmployeeSpecialResultModel> searchResult = new SearchBaseResultModel<EmployeeSpecialResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.ToListAsync();

            return searchResult;
        }

    }
}
