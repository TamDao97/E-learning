using Elearning.Model.Models.Answer;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class LessonQuestionModel
    {
        public string Id { get; set; }
        public string TopicId { get; set; }
        public string TopicName { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public bool? Status { get; set; }
        public int Index { get; set; }
        public List<AnswerInfoModel> ListAnswer { get; set; }
        public LessonQuestionModel()
        {
            ListAnswer = new List<AnswerInfoModel>();
        }
    }
}
