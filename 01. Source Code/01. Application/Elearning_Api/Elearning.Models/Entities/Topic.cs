using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class Topic
    {
        public Topic()
        {
            Question = new HashSet<Question>();
        }

        public string Id { get; set; }
        public string ParentTopicId { get; set; }
        public string Name { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
