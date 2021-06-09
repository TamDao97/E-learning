using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Category
{
    public class CategorySearchConditionModel : SearchBaseModel
    {
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
