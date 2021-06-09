using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class GroupFunction
    {
        public GroupFunction()
        {
            Permission = new HashSet<Permission>();
        }

        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }

        public virtual ICollection<Permission> Permission { get; set; }
    }
}
