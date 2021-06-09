using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Base
{
    public class SearchBaseStatusModel<T>
    {
        /// <summary>
        /// Tổng số đang tạo
        /// </summary>
        public int TotalCreating { get; set; }
        /// <summary>
        /// Tổng số yêu cầu duyệt
        /// </summary>
        public int TotalRequest { get; set; }
        /// <summary>
        /// Tổng số duyệt
        /// </summary>
        public int TotalApproval { get; set; }
        /// <summary>
        /// Tổng số hủy duyệt
        /// </summary>
        public int TotalNotApproval { get; set; }
        /// <summary>
        /// Tổng số không duyệt
        /// </summary>
        public int TotalNotBrowse { get; set; }
        public int TotalItems { get; set; }
        public List<T> DataResults { get; set; }
    }
}
