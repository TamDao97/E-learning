using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Models.Entities
{
    public partial class LessonFrameQuestion
    {
        [Key]
        [MaxLength(36)]
        public string Id { get; set; }
        [Required]
        [StringLength(36)]
        public string LessonFrameId { get; set; }
        [Required]
        [StringLength(36)]
        public string QuestionId { get; set; }
        public virtual LessonFrame LessonFrame { get; set; }
    }
}
