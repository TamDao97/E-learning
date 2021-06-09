using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.GroupUser
{
   public class GroupFunctionModel
    {
        /// <summary>
        /// Id nhóm 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Tên nhóm
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Mã nhóm
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Số quyền
        /// </summary>
        public int PermissionTotal { get; set; }

        /// <summary>
        /// Đếm
        /// </summary>
        public int CheckCount { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public bool? Status { get; set; }

        /// <summary>
        /// Check
        /// </summary>
        public bool Checked { get; set; }

        /// <summary>
        /// Danh sách quyền
        /// </summary>
        public List<PermissionModel> Permissions { set; get; }

        public GroupFunctionModel ()
        {
            Permissions = new List<PermissionModel>();
        }
    }
}
