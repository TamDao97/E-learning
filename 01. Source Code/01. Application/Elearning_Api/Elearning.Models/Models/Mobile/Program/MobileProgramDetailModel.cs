using System.Collections.Generic;

namespace Elearning.Model.Models.Mobile.Program
{
    public class MobileProgramDetailModel
    {
        /// <summary>
        /// Id chương trình đào tạo
        /// </summary>
        public string ProgramId { get; set; }
        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Danh sách khóa học
        /// </summary>
        public List<MobileCourseModel> Courses { get; set; }
    }
}
