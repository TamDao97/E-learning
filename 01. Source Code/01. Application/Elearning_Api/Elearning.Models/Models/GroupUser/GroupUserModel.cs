using Elearning.Models.GroupUser;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.GroupUser
{
    public class GroupUserModel
    {
        public string Id { get; set; }

        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = "Tên")]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public List<PermissionModel> ListPermission { get; set; }
        public GroupUserModel()
        {
            ListPermission = new List<PermissionModel>();
        }
    }
}
