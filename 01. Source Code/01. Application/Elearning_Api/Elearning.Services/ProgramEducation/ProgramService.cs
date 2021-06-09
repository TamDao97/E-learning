using Elearning.Model.Entities;
using Elearning.Model.Models.EducationProgram;
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
using NTS.Common.Helpers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.ProgramEducation
{
    public class ProgramService : IProgramService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IDetectionService _detection;


        public ProgramService (ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;

        }
        /// <summary>
        /// Thêm chương trình đào tạo
        /// </summary>
        /// <param name="programModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateProgramAsync (HttpRequest request, ProgramModel programModel, string userId)
        {
            var checkProgram = _sqlContext.Program.FirstOrDefault(u => u.Code.ToLower().Equals(programModel.Code.ToLower()));
            if (checkProgram != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Program);
            }
            var checkNameProgram = _sqlContext.Program.FirstOrDefault(u => u.Name.ToLower().Equals(programModel.Name.ToLower()));
            
            Program program = new Program()
            {
                Id = Guid.NewGuid().ToString(),
                Name = programModel.Name,
                Content = programModel.Content,
                Description = programModel.Description,
                ImagePath = programModel.ImagePath,
                Status = programModel.Status,
                Code=programModel.Code,
                Slug= checkNameProgram==null?SlugHelper.ConverNameToSlug(programModel.Name): SlugHelper.ConverNameToSlug(programModel.Name)+DateTime.Now.ToString("yyyyMMddHHmmss"),
                CreateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            _sqlContext.Program.Add(program);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới chương trình đào tạo: " + programModel.Name;
            LogService.Event(userHistory, _detection);
        }
        /// <summary>
        /// Xóa chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task DeleteProgramByIdAsync (HttpRequest request, string id, string userId)
        {
            var programExist = _sqlContext.Program.Find(id);
            var programName = _sqlContext.Program.Find(id).Name;
            if (programExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            var checkProgram = _sqlContext.Course.Where(s => s.ProgramId == id).Count();
            if (checkProgram > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Program);
            }
            _sqlContext.Program.Remove(programExist);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa chương trình đào tạo: " + programName;
            LogService.Event(userHistory, _detection);

        }
        /// <summary>
        /// Lấy thông tin chương trình đào tạo theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<ProgramInfoModel> GetProgramByIdAsync (string id, string userId)
        {
            var programInfo = (from a in _sqlContext.Program.AsNoTracking()
                               where a.Id.Equals(id)
                               select new ProgramInfoModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   Content = a.Content,
                                   ImagePath = a.ImagePath,
                                   Description = a.Description,
                                   Status = a.Status,
                                   Code=a.Code,
                               }).FirstOrDefault();

            if (programInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            return programInfo;
        }
        /// <summary>
        /// Tìm kiếm chương trình đào tạo
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<ProgramResultModel>> SearchProgramAsync (ProgramSearchModel searchModel)
        {
            var data = (from a in _sqlContext.Program.AsNoTracking()
                        select new ProgramResultModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            Status = a.Status,
                            Code=a.Code,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Description))
            {
                data = data.Where(r => r.Description.ToUpper().Contains(searchModel.Description.ToUpper()));
            }
            if (!string.IsNullOrEmpty(searchModel.Code))
            {
                data = data.Where(r => r.Code.ToUpper().Contains(searchModel.Code.ToUpper()));
            }
            SearchBaseResultModel<ProgramResultModel> searchResult = new SearchBaseResultModel<ProgramResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.OrderBy(s => s.Code).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            return searchResult;
        }
        /// <summary>
        /// cập nhập chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="programModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateProgramAsync (HttpRequest request, string id, ProgramModel programModel, string userId)
        {
            var program = await _sqlContext.Program.FindAsync(id);
            if (program == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            var programExist = _sqlContext.Program.AsNoTracking().FirstOrDefault(o => !o.Id.Equals(id) && o.Code.ToLower().Equals(programModel.Code.ToLower()));
            if (programExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0003, TextResourceKey.Program);
            }
            var checkNameProgram = _sqlContext.Program.FirstOrDefault(u => !u.Id.Equals(id)&& u.Name.ToLower().Equals(programModel.Name.ToLower()));

            string NameOld = program.Name.NTSTrim();

            program.Name = programModel.Name;
            program.Content = programModel.Content;
            program.Description = programModel.Description;
            program.ImagePath = programModel.ImagePath;
            program.Slug = checkNameProgram == null ? SlugHelper.ConverNameToSlug(programModel.Name) : SlugHelper.ConverNameToSlug(programModel.Name) + DateTime.Now.ToString("yyyyMMddHHmmss");
            program.UpdateBy = userId;
            program.UpdateDate = DateTime.Now;
            program.Status = programModel.Status;
            program.Code = programModel.Code;
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == programModel.Name.ToLower())
            {
                userHistory.Content = "Cập nhật chương trình đào tạo tên là: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật chương trình đào tạo có tên ban đầu là: " + NameOld + " thành " + programModel.Name;
            }
            LogService.Event(userHistory, _detection);
        }

        public async Task UpdateStatusProgramAsync (HttpRequest request, string id, string userId)
        {

            var programExist = _sqlContext.Program.Find(id);
            var programExistName = _sqlContext.Program.Find(id).Name;
            if (programExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            if (programExist.Status == true)
            {
                programExist.Status = false;
            }
            else
            {
                programExist.Status = true;
            }

            _sqlContext.SaveChanges();
            var statusName = this.getNameStatus(programExist.Status);

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái chương trình đào tạo: " + programExistName + " thành " + statusName;
            LogService.Event(userHistory, _detection);
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
