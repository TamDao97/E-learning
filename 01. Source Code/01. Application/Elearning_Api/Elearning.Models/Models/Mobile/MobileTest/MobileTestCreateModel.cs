using Elearning.Model.Models.Mobile.MobileAnswer;
using Elearning.Model.Models.Mobile.MobileQuestion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileTest
{
    public class MobileTestCreateModel
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
        public List<MobileAnswerLearnerModel> ListAnswer { get; set; }

        public MobileTestCreateModel()
        {
            ListAnswer = new List<MobileAnswerLearnerModel>();
        }
    }
}
