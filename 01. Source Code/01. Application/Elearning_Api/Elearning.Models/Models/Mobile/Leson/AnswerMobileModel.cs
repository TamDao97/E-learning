namespace Elearning.Model.Models.Mobile.Leson
{
    public class AnswerMobileModel
    {
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerLable { get; set; }
        public int DisplayIndex { get; set; }
        public bool IsCorrect { get; set; }
        public string LearnerAnswerContent { get; set; }
        public int LearnerDisplayIndex { get; set; }
        public bool LearnerIsCorrect { get; set; }
    }
}
