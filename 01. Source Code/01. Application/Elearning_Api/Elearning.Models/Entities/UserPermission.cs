using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class UserPermission
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public string PermissionId { get; set; }

        public virtual Permission Permission { get; set; }
        public virtual User User { get; set; }
    }
}
