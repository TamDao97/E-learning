using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class FinishTestCreateModel
    {
        /// <summary>
        /// Id khóa học
        /// </summary>
        public string CourseId { get; set; }

        /// <summary>
        /// Id người học
        /// </summary>
        public string LearnerId { get; set; }

        /// <summary>
        /// Id bài giảng
        /// </summary>
        public string LessonId { get; set; }


        /// <summary>
        /// Loại câu hỏi
        /// </summary>
        //public int Type { get; set; }


        public List<QuestionFrontendModel> ListQuestion { get; set; }

        public FinishTestCreateModel()
        {
            ListQuestion = new List<QuestionFrontendModel>();
        }
    }
}
