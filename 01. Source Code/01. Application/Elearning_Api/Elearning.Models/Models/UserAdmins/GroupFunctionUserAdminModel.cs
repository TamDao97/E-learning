using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.UserAdmin
{
    public class GroupFunctionUserAdminModel
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Index { get; set; }
        public string GroupFunctionId { get; set; }
        public int PermissionTotal { get; set; }
        public int CheckCount { get; set; }
        public bool IsChecked { get; set; }
        public List<PermissionsModel> Permissions { set; get; }
        public GroupFunctionUserAdminModel()
        {
            Permissions = new List<PermissionsModel>();
        }
    }
}
