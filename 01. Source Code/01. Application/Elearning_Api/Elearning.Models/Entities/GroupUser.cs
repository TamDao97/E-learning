using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class GroupUser
    {
        public GroupUser()
        {
            GroupPermission = new HashSet<GroupPermission>();
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDisable { get; set; }
        public string Description { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }

        public virtual ICollection<GroupPermission> GroupPermission { get; set; }
    }
}
