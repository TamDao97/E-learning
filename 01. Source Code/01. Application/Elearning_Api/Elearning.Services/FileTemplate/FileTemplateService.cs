using Elearning.Model.Models.FileTemplate;
using Elearning.Model.Models.UserHistory;
using Elearning.Models;
using Elearning.Models.Entities;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Files;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.FileTemplate
{
    public class FileTemplateService : IFileTemplateService
    {
        private readonly ElearningContext sqlContext;
        private readonly IDetectionService _detection;

        public FileTemplateService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this._detection = _detection;
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Tạo mới mẫu
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateAsync(HttpRequest request, FileTemplateModel model, string userId)
        {
            bool isExistCode = await sqlContext.FileTemplate.AsNoTracking().AnyAsync(r => r.Code.Equals(model.Code));

            if (isExistCode)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0037, "Mã mẫu");
            }

            Model.Entities.FileTemplate fileTemplate = new Model.Entities.FileTemplate
            {
                Id = Guid.NewGuid().ToString(),
                Code = model.Code,
                Name = model.Name,
                FilePath = model.FilePath,
                Index = model.Index,
                Description = model.Description,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                Type = true,
                UpdateDate = DateTime.Now,

            };

            try
            {
                sqlContext.FileTemplate.Add(fileTemplate);
                sqlContext.SaveChanges();
                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                userHistory.Content = "Thêm mới mẫu chứng chỉ: " + model.Name;
                LogService.Event(userHistory, _detection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Xóa mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteAsync(HttpRequest request, string id, string userId)
        {
            var modelDelete = await sqlContext.FileTemplate.FindAsync(id);
            var programName = modelDelete.Name;
            if (modelDelete == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0019, "Mẫu");
            }

            sqlContext.Remove(modelDelete);
            try
            {
                sqlContext.SaveChanges();
                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                userHistory.Content = "Xóa mẫu chứng chỉ: " + programName;
                LogService.Event(userHistory, _detection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Chi tiết mẫu
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Model.Entities.FileTemplate> GetByIdAsync(string id)
        {
            Model.Entities.FileTemplate model = await sqlContext.FileTemplate.FindAsync(id);

            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0019, "Mẫu");
            }

            return model;
        }

        /// <summary>
        /// Lấy danh sách
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<List<FileTemplateModel>> SearchAsync(bool type)
        {
            List<FileTemplateModel> listData = await (from f in sqlContext.FileTemplate.AsNoTracking()
                                                      where f.Type.Equals(type)
                                                      join u in sqlContext.User.AsNoTracking() on f.CreateBy equals u.Id
                                                      select new FileTemplateModel
                                                      {
                                                          Id = f.Id,
                                                          Code = f.Code,
                                                          Name = f.Name,
                                                          Description = f.Description,
                                                          FilePath = f.FilePath,
                                                          CreateBy = u.UserName,
                                                          UpdateDate = f.UpdateDate
                                                      }).ToListAsync();

            return listData;
        }

        /// <summary>
        /// Cập nhập
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(HttpRequest request, FileTemplateModel model, string userId)
        {
            var modelEdit = sqlContext.FileTemplate.Find(model.Id);
            string NameOld = modelEdit.Name;
            bool isExistCode = await sqlContext.FileTemplate.AsNoTracking().AnyAsync(r => r.Code.Equals(model.Code) && !modelEdit.Code.Equals(model.Code));

            if (isExistCode)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0037, "Mã mẫu");
            }

            modelEdit.Code = model.Code;
            modelEdit.Name = model.Name;
            modelEdit.Description = model.Description;
            modelEdit.Index = model.Index;
            modelEdit.Type = model.Type;
            modelEdit.UpdateBy = userId;
            modelEdit.UpdateDate = DateTime.Now;

            if (!modelEdit.FilePath.Contains(model.FilePath))
            {
                modelEdit.FilePath = model.FilePath;
            }

            try
            {
                sqlContext.SaveChanges();
                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

                if(model.Type == true)
                {
                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        userHistory.Content = "Cập nhật mẫu chứng chỉ tên là: " + NameOld;
                    }
                    else
                    {
                        userHistory.Content = "Cập nhật mẫu chứng chỉ có tên ban đầu là: " + NameOld + " thành " + model.Name;
                    }
                }
                else
                {
                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        userHistory.Content = "Cập nhật mẫu văn bản tên là: " + NameOld;
                    }
                    else
                    {
                        userHistory.Content = "Cập nhật mẫu văn bản có tên ban đầu là: " + NameOld + " thành " + model.Name;
                    }
                }


                LogService.Event(userHistory, _detection);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
