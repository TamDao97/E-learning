using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Model.Entities
{
    public partial class AnswerLearner
    {
        [Key]
        public long Id { get; set; }
        public string TestId { get; set; }
        [Required]
        [StringLength(36)]
        public string QuestionId { get; set; }
        [Required]
        [StringLength(36)]
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public int? DisplayIndex { get; set; }
        public bool? IsCorrect { get; set; }
        public virtual Test Test { get; set; }
    }
}
