using Elearning.Model.Models.Mobile.MobileAnswer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileQuestion
{
    public class MobileQuestionModel
    {
        /// <summary>
        /// Id câu hỏi
        /// </summary>
        public string QuestionId { get; set; }

        /// <summary>
        /// Loại câu hỏi
        /// 1: Câu hỏi đúng sai
        /// 2: Câu hỏi một đáp án đúng
        /// 3: Câu hỏi nhiều đáp án đúng
        /// 4: Câu hỏi sắp xếp
        /// 5: Câu hỏi điền vào chỗ trống.
        /// </summary>
        public int Type { get; set; }

        public List<MobileAnswerLearnerModel> ListAnswer { get; set; }

        public MobileQuestionModel()
        {
            ListAnswer = new List<MobileAnswerLearnerModel>();
        }

    }
}
