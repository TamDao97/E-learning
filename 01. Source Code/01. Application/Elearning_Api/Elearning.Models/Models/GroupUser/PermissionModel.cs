using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.GroupUser
{
    public class PermissionModel
    {

        /// <summary>
        /// Id quyền
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public string GroupFunctionId { get; set; }

        /// <summary>
        /// Mã quyền
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên quyền
        /// </summary>
        public string Name { get; set; }
        public string ScreenCode { get; set; }

        /// <summary>
        /// check
        /// </summary>
        public bool IsChecked { get; set; }

        public string Index { get; set; }
        public string FunctionId { get; set; }
        public string GroupFunctionName { get; set; }
        public string FunctionCode { get; set; }
        public string FunctionName { get; set; }
        public bool Checked { get; set; }
    }
}
