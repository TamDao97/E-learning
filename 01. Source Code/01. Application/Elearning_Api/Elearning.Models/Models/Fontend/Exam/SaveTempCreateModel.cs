using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class SaveTempCreateModel
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
        /// Id câu hỏi
        /// </summary>
        public string QuestionId { get; set; }
        public List<AnserLearnerModel> ListAnswer { get; set; }

        public SaveTempCreateModel()
        {
            ListAnswer = new List<AnserLearnerModel>();
        }
    }
}
