using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class QuestionSearchConditionModel
    {
        public string TopicId { get; set; }
        public string Name { get; set; }
        public int? Type { get; set; }
        public List<string> ListId { get; set; }
        public QuestionSearchConditionModel()
        {
            ListId = new List<string>();
        }
    }
}
