using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Question
{
    public class QuestionsSearchConditionModel : SearchBaseModel
    {
        /// <summary>
        /// Chủ đề
        /// </summary>
        public string TopicId { get; set; }

        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Loại câu hỏi
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// Trạng thái câu hỏi
        /// </summary>
        public bool? Status { get; set; }
        public int? ApprovalStatus { get; set; }
    }
}
