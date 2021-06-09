using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class GroupPermission
    {
        public string Id { get; set; }
        public string GroupUserId { get; set; }
        public string PermissionId { get; set; }

        public virtual GroupUser GroupUser { get; set; }
        public virtual Permission Permission { get; set; }
    }
}
