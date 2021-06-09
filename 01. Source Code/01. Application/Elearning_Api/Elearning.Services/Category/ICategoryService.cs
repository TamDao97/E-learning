using Elearning.Model.Models.Category;
using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Categorys
{
    public interface ICategoryService
    {
        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<CategorySearchResultModel>> SearchCategoryAsync(CategorySearchConditionModel searchModel);

        /// <summary>
        /// Thêm danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateCategoryAsync(CategoryCreateModel model, string userId);

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateCategoryAsync(string id, CategoryCreateModel model, string userId);

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task DeleteCategoryByIdAsync(string id);

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<CategoryCreateModel> GetCategoryByIdAsync(string id, string userId);
    }
}
