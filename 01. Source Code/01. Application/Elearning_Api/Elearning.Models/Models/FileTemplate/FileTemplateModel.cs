using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.FileTemplate
{
    public class FileTemplateModel
    {
        public string Id { get; set; }

        /// <summary>
        /// Mã mẫu
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Tên mẫu
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Đường dẫn file 
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// Thứ tự ưu tiên
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreateBy { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreateDate { get; set; }

        /// <summary>
        /// Người cập nhập
        /// </summary>
        public string UpdateBy { get; set; }

        /// <summary>
        /// Ngày cập nhập
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// Loại mẫu 
        /// </summary>
        public bool Type { get; set; }
    }
}
