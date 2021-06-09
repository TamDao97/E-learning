using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Category
{
    public class CategorySearchResultModel
    {
        /// <summary>
        /// Id danh mục
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// Id cha
        /// </summary>
        public string ParentCategoryId { get; set; }
        /// <summary>
        /// Tên danh mục
        /// </summary>
        public string Name { get; set; }
    }
}
