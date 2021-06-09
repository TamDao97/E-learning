using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.HomeSillder
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeSilderCreateModel
    {

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// Thứ tự hiển thị
        /// </summary>
        public int DisplayIndex { get; set; }
        public bool Status { get; set; }

    }
}
