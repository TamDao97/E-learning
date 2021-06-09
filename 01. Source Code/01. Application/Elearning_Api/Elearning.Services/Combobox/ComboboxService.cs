using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Elearning.Models.Combobox;
using Elearning.Models.Entities;
using RestSharp;
using Elearning.Models.Settings;
using Newtonsoft.Json;
using Elearning.Models;
using Microsoft.Extensions.Options;
using Elearning.Models.Base;
using NTS.Common;

namespace Elearning.Services.Combobox
{
    public class ComboboxService : IComboboxService
    {
        private readonly ElearningContext sqlContext;
        private readonly AppSettingModel _appSettingModel;

        public ComboboxService(ElearningContext sqlContext, IOptions<AppSettingModel> appSettingOptionss)
        {
            this.sqlContext = sqlContext;
            this._appSettingModel = appSettingOptionss.Value;
        }

        public async Task<ApiResultModel> GetAllBarcodeType()
        {
            var apiUrl = _appSettingModel.ServerApiUrl;
            var token = _appSettingModel.Token;
            var client = new RestClient(apiUrl + "api/combobox/get-barcodetypes");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            var response = client.Execute(request);
            ApiResultModel resultModel = new ApiResultModel();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            else
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            return resultModel;
        }

        public async Task<ApiResultModel> GetAllCodeType()
        {
            var apiUrl = _appSettingModel.ServerApiUrl;
            var token = _appSettingModel.Token;
            var client = new RestClient(apiUrl + "api/combobox/get-codeTypes");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            var response = client.Execute(request);
            ApiResultModel resultModel = new ApiResultModel();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            else
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            return resultModel;
        }

        public async Task<ApiResultModel> GetAllParkingType()
        {
            var apiUrl = _appSettingModel.ServerApiUrl;
            var token = _appSettingModel.Token;
            var client = new RestClient(apiUrl + "api/combobox/get-parkingTypes");
            var request = new RestRequest();
            request.Method = Method.GET;
            request.AddHeader("Content-Type", "application/json");
            request.AddParameter("Authorization", "Bearer " + token, ParameterType.HttpHeader);
            var response = client.Execute(request);
            ApiResultModel resultModel = new ApiResultModel();
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            else
            {
                resultModel = JsonConvert.DeserializeObject<ApiResultModel>(response.Content);
            }
            return resultModel;
        }
        /// <summary>
        /// Danh sách chương trình
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetAllProgram()
        {
            var data = await (from a in sqlContext.Program.AsNoTracking()
                              where a.Status == Constants.Program_Status_Show
                              select new ComboboxModel
                              {
                                  Id = a.Id,
                                  Name = a.Name
                              }).OrderBy(s=>s.Name).ToListAsync();
            return data;
        }

        /// <summary>
        /// Danh sách nhóm tài khoản
        /// </summary>
        /// <returns></returns>
        public List<ComboboxModel> GetListGroupuser()
        {
            List<ComboboxModel> result = new List<ComboboxModel>();
            result = (from c in sqlContext.GroupUser
                      where c.IsDisable == true
                      select new ComboboxModel
                      {
                          Id = c.Id,
                          Name = c.Name
                      }).ToList();
            return result;
        }

        /// <summary>
        /// Danh sách chủ đề
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxParentModel>> GetCategoryAsync()
        {
            var data = await (from a in sqlContext.Category.AsNoTracking()
                              select new ComboboxParentModel
                              {
                                  Id = a.Id,
                                  ParentId = a.ParentCategoryId,
                                  Name = a.Name
                              }).ToListAsync();
            return data;
        }

        /// <summary>
        /// Danh sách danh mục
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxTopicFullModel>> GetCategoryFullAsync()
        {
            List<ComboboxTopicFullModel> listCategoryRoot = sqlContext.Category.AsNoTracking().Where(r => string.IsNullOrEmpty(r.ParentCategoryId)).Select(r => new ComboboxTopicFullModel
            {
                Key = r.Id,
                Title = r.Name
            }).ToList();

            List<ComboboxParentModel> listCategory = sqlContext.Category.AsNoTracking().Where(r => !string.IsNullOrEmpty(r.ParentCategoryId)).Select(r => new ComboboxParentModel
            {
                Id = r.Id,
                ParentId = r.ParentCategoryId,
                Name = r.Name
            }).ToList();

            foreach (var item in listCategoryRoot)
            {
                item.Children = await GetListTopicChild(item, listCategory);
            }

            return listCategoryRoot;
        }

        /// <summary>
        /// Danh sách chủ đề
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxParentModel>> GetTopicAsync()
        {
            var data = await (from a in sqlContext.Topic.AsNoTracking()
                              select new ComboboxParentModel
                              {
                                  Id = a.Id,
                                  ParentId = a.ParentTopicId,
                                  Name = a.Name
                              }).ToListAsync();
            return data;
        }

        /// <summary>
        /// Danh sách topic
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxTopicFullModel>> GetTopicFullAsync()
        {
            List<ComboboxTopicFullModel> listTopicRoot = sqlContext.Topic.AsNoTracking().Where(r => string.IsNullOrEmpty(r.ParentTopicId)).Select(r => new ComboboxTopicFullModel
            {
                Key = r.Id,
                Title = r.Name
            }).ToList();

            List<ComboboxParentModel> listTopic = sqlContext.Topic.AsNoTracking().Where(r => !string.IsNullOrEmpty(r.ParentTopicId)).Select(r => new ComboboxParentModel
            {
                Id = r.Id,
                ParentId = r.ParentTopicId,
                Name = r.Name
            }).ToList();

            foreach (var item in listTopicRoot)
            {
                item.Children = await GetListTopicChild(item, listTopic);
            }

            return listTopicRoot;
        }

        /// <summary>
        /// danh sách các topic con
        /// </summary>
        /// <param name="parentId"></param>
        /// <param name="listTopic"></param>
        /// <returns></returns>
        public async Task<List<ComboboxTopicFullModel>> GetListTopicChild(ComboboxTopicFullModel parent, List<ComboboxParentModel> listTopic)
        {
            List<ComboboxTopicFullModel> listTopicChild = new List<ComboboxTopicFullModel>();
            listTopicChild = listTopic.Where(r => r.ParentId.Equals(parent.Key)).Select(r => new ComboboxTopicFullModel
            {
                Key = r.Id,
                Title = r.Name
            }).ToList();

            if (listTopicChild.FirstOrDefault() == null)
                parent.IsLeaf = true;

            foreach (var item in listTopicChild)
            {
                item.Children = await GetListTopicChild(item, listTopic);
            }

            return listTopicChild;
        }


        /// <summary>
        /// Số thứ tự slider 
        /// </summary>
        /// <returns></returns>
        public async Task<List<CbbOrderModel>> SearchHomeSlider()
        {
            List<CbbOrderModel> result = new List<CbbOrderModel>();
            result = (from a in sqlContext.HomeSlider.AsNoTracking()
                      orderby a.DisplayIndex
                      select new CbbOrderModel
                      {
                          Id = a.Id,
                          Order = a.DisplayIndex,
                      }).ToList();

            if (result.Count() == 0)
            {
                CbbOrderModel addFirstIndex = new CbbOrderModel();
                addFirstIndex.Id = 0;
                addFirstIndex.Order = 1;
                result.Add(addFirstIndex);
            }
            else
            {
                var maxIndex = sqlContext.HomeSlider.AsNoTracking().Select(b => b.DisplayIndex).Max();
                CbbOrderModel addIndex = new CbbOrderModel();
                addIndex.Id = 0;
                addIndex.Order = (maxIndex + 1);
                result.Add(addIndex);
            }
            return result;
        }

        /// <summary>
        /// Danh sách chuyên gia
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxModel>> GetEmployeeAsync()
        {
            List<ComboboxModel> result = new List<ComboboxModel>();
            result = (from a in sqlContext.Employee.AsNoTracking()
                      join b in sqlContext.User.AsNoTracking() on a.Id equals b.ObjectId
                      where b.Type == 2
                      select new ComboboxModel
                      {
                          Id = a.Id,
                          Name = a.Name,
                      }).ToList();
            return result;
        }

        public async Task<List<ComboboxIntModel>> GetHomeSpecialistAsync()
        {
            List<ComboboxIntModel> result = new List<ComboboxIntModel>();
            result = (from a in sqlContext.HomeSpecialist.AsNoTracking()
                      select new ComboboxIntModel
                      {
                          Id = a.Id,
                          Name = a.Title,
                      }).ToList();
            return result;
        }

        public async Task<SearchBaseResultModel<ComboboxModel>> GetListProvince()
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.Province.AsNoTracking()
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.ProvinceId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }


        public async Task<SearchBaseResultModel<ComboboxIntModel>> GetListNation()
        {
            SearchBaseResultModel<ComboboxIntModel> searchResult = new SearchBaseResultModel<ComboboxIntModel>();
            var province = (from r in sqlContext.Nation.AsNoTracking()
                            orderby r.Name
                            select new ComboboxIntModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }


        public async Task<SearchBaseResultModel<ComboboxModel>> GetListDistrictByProvinceId(string ProvinceId)
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.District.AsNoTracking()
                            where r.ProvinceId.Equals(ProvinceId)
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.DistrictId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }


        public async Task<SearchBaseResultModel<ComboboxModel>> GetListWardByDistrictId(string DistrictId)
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.Ward.AsNoTracking()
                            where r.DistrictId.Equals(DistrictId)
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.WardId,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();


            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }

        public async Task<SearchBaseResultModel<ComboboxModel>> GetUser()
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.User.AsNoTracking()
                            orderby r.UserName
                            select new ComboboxModel()
                            {
                                Id = r.Id,
                                Name = r.UserName,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();

            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }

        public async Task<SearchBaseResultModel<ComboboxModel>> GetLearner()
        {
            SearchBaseResultModel<ComboboxModel> searchResult = new SearchBaseResultModel<ComboboxModel>();
            var province = (from r in sqlContext.Learner.AsNoTracking()
                            orderby r.Name
                            select new ComboboxModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                            }
                         ).AsQueryable();

            searchResult.DataResults = province.ToList();

            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách đơn vị chủ quản
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComboboxUnitModel>> GetListManageUnits()
        {
            var manageUnits = (from r in sqlContext.ManagerUnit.AsNoTracking()
                            orderby r.Name
                            select new ComboboxUnitModel()
                            {
                                Id = r.Id,
                                Name = r.Name,
                                Level = r.Level,
                            }
                         ).ToList();

            // Trả về kết quả thực hiện thành công, và danh sách kết quả tìm kiếm được

            return manageUnits;
        }
    }
}
