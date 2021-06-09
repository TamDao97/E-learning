using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Models.Entities
{
    public partial class LessonAnswerLearner
    {
        [Key]
        public long Id { get; set; }
        public long LessonFrameHistoryId { get; set; }
        [Required]
        [StringLength(36)]
        public string QuestionId { get; set; }
        [Required]
        [StringLength(36)]
        public string AnswerId { get; set; }
        public string AnswerContent { get; set; }
        public bool? IsCorrect { get; set; }
        public int? DisplayIndex { get; set; }
        public virtual LessonFrameHistory LessonFrameHistory { get; set; }
    }
}
