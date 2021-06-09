using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileLesson
{
    public class MobileLessonModel
    {
        /// <summary>
        /// Id bài học 
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Id khóa học
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// Tên bài học
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Loại bài học
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// true: ddax hocj
        /// false: chua hoc
        /// </summary>
        public bool Status { get; set; }

        /// <summary>
        /// Danh sách bài giảng chi tiết
        /// </summary>
        public List<MobileLessonFrameModel> ListLessonFrame { get; set; }
        public MobileLessonModel()
        {
            ListLessonFrame = new List<MobileLessonFrameModel>();
        }
    }

    public class MobileLessonFrameModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Type { get; set; }
        public bool Status { get; set; }
    }
}
