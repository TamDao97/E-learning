using System;
using System.Collections.Generic;

namespace Elearning.Models.Entities
{
    public partial class User
    {
        public User()
        {
            UserPermission = new HashSet<UserPermission>();
        }

        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string ManagerUnitId { get; set; }
        public string UserName { get; set; }
        public string GroupUserId { get; set; }
        public string SecurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public bool IsDisable { get; set; }
        public bool IsLogin { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public int Type { get; set; }
        public int ManagerUnitLevel { get; set; }

        public virtual ICollection<UserPermission> UserPermission { get; set; }
    }
}
