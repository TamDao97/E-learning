using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Models.GroupUser
{
    public class GroupFunctionCreateModel
    {

        /// <summary>
        /// Tên nhóm quyền
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Name, ResourceType = typeof(TextResource))]
        [MaxLength(150, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }

        /// <summary>
        /// Trạng thái nhóm quyền
        /// </summary>
        public bool IsDisable { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// List quyền
        /// </summary>
        public List<PermissionModel> ListPermission { get; set; }

        public GroupFunctionCreateModel ()
        {
            ListPermission = new List<PermissionModel>();
        }
    }
}