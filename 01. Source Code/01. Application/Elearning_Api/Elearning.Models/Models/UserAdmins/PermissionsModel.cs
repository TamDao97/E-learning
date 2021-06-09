using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.UserAdmin
{
    public class PermissionsModel
    {
        public string Id { get; set; }
        public string GroupFunctionId { get; set; }
        public string PermissionId { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool IsChecked { get; set; }
    }
}
