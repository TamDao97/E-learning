using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    public class ManagerUnit
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Logo { get; set; }
        public int Level { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

    }
}
