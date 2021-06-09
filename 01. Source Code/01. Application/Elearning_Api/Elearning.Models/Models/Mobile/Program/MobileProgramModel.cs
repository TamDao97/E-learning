using System.Collections.Generic;

namespace Elearning.Model.Models.Mobile.Program
{
    public class MobileProgramModel
    {
        /// <summary>
        /// Id Chương trình
        /// </summary>
        public string ProgramId { get; set; }
        /// <summary>
        /// Tên chương trình
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Danh sách khóa học
        /// </summary>
        public List<MobileCourseModel> Courses { get; set; }

    }
}
