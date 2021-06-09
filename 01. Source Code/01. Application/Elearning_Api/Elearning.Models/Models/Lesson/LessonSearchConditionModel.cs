using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class LessonSearchConditionModel: SearchBaseModel
    {
        /// <summary>
        /// Tên bài giảng
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Chủ đề
        /// </summary>
        public string CategoryId { get; set; }
        /// <summary>
        /// Tình trạng
        /// </summary>
        public bool? Status { get; set; }
        /// <summary>
        /// Loại
        /// </summary>
        public int? Type { get; set; }
        /// <summary>
        /// Đơn vị chủ quản
        /// </summary>
        public string ManageUnitId { get; set; }
        /// <summary>
        /// Người tạo
        /// </summary>
        public string CreateBy { get; set; }
        public int? ApprovalStatus { get; set; }
    }
}
