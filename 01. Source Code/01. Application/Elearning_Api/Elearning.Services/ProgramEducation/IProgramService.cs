using Elearning.Model.Models.EducationProgram;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.ProgramEducation
{
    public interface IProgramService
    {
        /// <summary>
        /// Tìm kiếm chương trình đào tạo
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<ProgramResultModel>> SearchProgramAsync (ProgramSearchModel searchModel);
        /// <summary>
        /// Lấy thông tin chương trình đào tạo theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<ProgramInfoModel> GetProgramByIdAsync (string id, string userId);
        /// <summary>
        /// Thêm chương trình đào tạo
        /// </summary>
        /// <param name="programModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateProgramAsync (HttpRequest request, ProgramModel programModel, string userId);
        /// <summary>
        /// Cập nhập chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="ProgramModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateProgramAsync (HttpRequest request, string id, ProgramModel programModel, string userId);
        /// <summary>
        /// Xóa chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteProgramByIdAsync (HttpRequest request, string id, string userId);
        Task UpdateStatusProgramAsync (HttpRequest request, string id, string userId);
    }
}
