using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.Base
{
    public class SearchBaseModel
    {
        /// <summary>
        /// Số bán ghi trên trang
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Trang hiện tại
        /// </summary>
        public int PageNumber { get; set; }

        public string OrderBy { get; set; }

        public bool OrderType { get; set; }

        /// <summary>
        /// Từ ngày
        /// </summary>
        public DateTime? DateFrom { get; set; }

        /// <summary>
        /// Đến ngày
        /// </summary>
        public DateTime? DateTo { get; set; }
    }
}
