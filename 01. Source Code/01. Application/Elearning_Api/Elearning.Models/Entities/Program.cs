using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class Program
    {
        public Program ()
        {
            Course= new HashSet<Course>();
        }
        public string Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public string Slug { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual ICollection<Course> Course { get; set; }
    }
}
