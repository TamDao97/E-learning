using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class Category
    {
        public Category()
        {
            Lesson = new HashSet<Lesson>();
        }

        public string Id { get; set; }
        public string ParentCategoryId { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<Lesson> Lesson { get; set; }
    }
}
