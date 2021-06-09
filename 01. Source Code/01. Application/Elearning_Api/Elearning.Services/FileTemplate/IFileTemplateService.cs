using Elearning.Model.Models.FileTemplate;
using Elearning.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.FileTemplate
{
    public interface IFileTemplateService
    {
        /// <summary>
        /// Tìm kiếm
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<List<FileTemplateModel>> SearchAsync(bool type);

        /// <summary>
        /// Tạo mới
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateAsync(HttpRequest request, FileTemplateModel model, string userId);

        /// <summary>
        /// Cập nhập
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateAsync(HttpRequest request, FileTemplateModel model, string userId);

        /// <summary>
        /// Xóa mẫu 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteAsync(HttpRequest request, string id, string userId);

        /// <summary>
        /// Lấy chi tiết
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Model.Entities.FileTemplate> GetByIdAsync(string id);
    }
}
