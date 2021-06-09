using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.GroupUser
{
    public class FuntionModel
    {
        /// <summary>
        /// Id quyền
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Mã quyền
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên quyền
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Id nhóm quyền
        /// </summary>
        public string GroupFunctionId { get; set; }

        /// <summary>
        /// Trạng thái đã chọn chưa
        /// </summary>
        public bool Checked { get; set; }
    }
}
