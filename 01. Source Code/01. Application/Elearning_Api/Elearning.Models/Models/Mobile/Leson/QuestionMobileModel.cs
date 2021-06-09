using System.Collections.Generic;

namespace Elearning.Model.Models.Mobile.Leson
{
    public class QuestionMobileModel
    {
        public string QuestionId { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public List<AnswerMobileModel> ListAnswer { get; set; }

        public bool IsResultLearner { get; set; }
    }
}
