using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class Answer
    {
        [Key]
        public string Id { get; set; }
        public string QuestionId { get; set; }
        public string AnswerContent { get; set; }
        public string AnswerLabel { get; set; }
        public bool IsCorrect { get; set; }
        public int DisplayIndex { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public virtual Question Question { get; set; }
    }
}
