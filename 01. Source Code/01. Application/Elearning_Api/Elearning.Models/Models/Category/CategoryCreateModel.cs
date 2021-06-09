using NTS.Common.Resource;
using System.ComponentModel.DataAnnotations;

namespace Elearning.Model.Models.Category
{
    public class CategoryCreateModel
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
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Name, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
    }
}
