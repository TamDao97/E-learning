using Elearning.Model.Models.Category;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace Elearning.Services.Categorys
{
    public class CategoryService : ICategoryService
    {
        private readonly ElearningContext sqlContext;

        public CategoryService(ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Tìm kiếm danh mục
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<CategorySearchResultModel>> SearchCategoryAsync(CategorySearchConditionModel searchModel)
        {
            var data = (from a in sqlContext.Category.AsNoTracking()
                        select new CategorySearchResultModel
                        {
                            Id = a.Id,
                            ParentCategoryId = a.ParentCategoryId,
                            Name = a.Name,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(i => i.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ParentCategoryId))
            {
                data = data.Where(i => i.ParentCategoryId.Equals(searchModel.ParentCategoryId));
            }

            SearchBaseResultModel<CategorySearchResultModel> searchResult = new SearchBaseResultModel<CategorySearchResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.Name).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            return searchResult;
        }

        /// <summary>
        /// Thêm danh mục
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateCategoryAsync(CategoryCreateModel model, string userId)
        {
            var checkCategory = sqlContext.Category.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.ToLower()) && u.ParentCategoryId.Equals(model.ParentCategoryId));
            if (checkCategory != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Category);
            }

            Category category = new Category()
            {
                Id = Guid.NewGuid().ToString(),
                ParentCategoryId = model.ParentCategoryId,
                Name = model.Name,
                CreateBy = userId,
                CreateDate = DateTime.Now,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            sqlContext.Category.Add(category);
            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Cập nhật danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateCategoryAsync(string id, CategoryCreateModel model, string userId)
        {
            var category = await sqlContext.Category.FirstOrDefaultAsync(i => i.Id.Equals(id));
            if (category == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Category);
            }

            var categoryExist = sqlContext.Category.AsNoTracking().FirstOrDefault(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower()) && o.ParentCategoryId.Equals(model.ParentCategoryId));
            if (categoryExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Category);
            }

            category.ParentCategoryId = model.ParentCategoryId;
            category.Name = model.Name;
            category.UpdateBy = userId;
            category.UpdateDate = DateTime.Now;

            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy thông tin danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CategoryCreateModel> GetCategoryByIdAsync(string id, string userId)
        {
            var resultInfo = await (from a in sqlContext.Category.AsNoTracking()
                                    where a.Id.Equals(id)
                                    select new CategoryCreateModel()
                                    {
                                        Id = a.Id,
                                        ParentCategoryId = a.ParentCategoryId,
                                        Name = a.Name,
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Category);
            }

            return resultInfo;
        }

        /// <summary>
        /// Xóa danh mục
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCategoryByIdAsync(string id)
        {
            var list = await sqlContext.Category.ToListAsync();
            var categoryExist = list.FirstOrDefault(i => i.Id.Equals(id));
            if (categoryExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Category);
            }

            var lesson = sqlContext.Lesson.FirstOrDefault(i => i.CategoryId.Equals(id));
            if (lesson != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Category);
            }

            var data = GetListCategoryChild(id, list);
            if (data.Count > 0)
            {
                var check = sqlContext.Lesson.FirstOrDefault(i => data.Select(a => a.Id).Contains(i.CategoryId));
                if (check != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.CategoryChild);
                }
            }

            sqlContext.Category.RemoveRange(data);
            sqlContext.Category.Remove(categoryExist);
            await sqlContext.SaveChangesAsync();
        }

        public List<Category> GetListCategoryChild(string parentId, List<Category> list)
        {
            List<Category> categories = new List<Category>();

            var data = list.Where(i => !string.IsNullOrEmpty(i.ParentCategoryId) && i.ParentCategoryId.Equals(parentId));
            categories.AddRange(data);
            foreach (var item in data)
            {
                categories.AddRange(GetListCategoryChild(item.Id, list));
            }

            return categories;
        }
    }
}
