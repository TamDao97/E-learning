using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class ExamQuestionModel
    {
        /// <summary>
        /// Id người dùng
        /// </summary>
        public string LearnerId { get; set; }

        /// <summary>
        /// Id bài kiểm tra
        /// </summary>
        public string LessonId { get; set; }

        /// <summary>
        /// Id khóa học
        /// </summary>
        public string CourseId { get; set; }
        public int Type { get; set; }


    }
}
