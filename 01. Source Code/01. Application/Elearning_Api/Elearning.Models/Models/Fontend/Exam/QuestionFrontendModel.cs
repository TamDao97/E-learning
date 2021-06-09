using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Exam
{
    public class QuestionFrontendModel
    {
        /// <summary>
        /// Id câu hỏi
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Loại câu hỏi
        /// 1: Câu hỏi đúng sai
        /// 2: Câu hỏi một đáp án đúng
        /// 3: Câu hỏi nhiều đáp án đúng
        /// 4: Câu hỏi sắp xếp
        /// 5: Câu hỏi điền vào chỗ trống.
        /// </summary>
        public int Type { get; set; }

        public List<AnserLearnerModel> ListAnswer { get; set; }

        public QuestionFrontendModel()
        {
            ListAnswer = new List<AnserLearnerModel>();
        }
    }
}
