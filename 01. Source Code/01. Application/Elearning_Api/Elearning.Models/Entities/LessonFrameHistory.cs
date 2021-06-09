using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Models.Entities
{
    public partial class LessonFrameHistory
    {
        public LessonFrameHistory()
        {
            LessonAnswerLearner = new HashSet<LessonAnswerLearner>();
        }

        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(36)]
        public string LessonFrameId { get; set; }
        [Required]
        [StringLength(36)]
        public string LessonId { get; set; }
        [Required]
        [StringLength(36)]
        public string LearnerId { get; set; }
        [Required]
        [StringLength(36)]
        public string CourseId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FinishDate { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalCorrect { get; set; }
        public bool Pass { get; set; }
        public virtual LessonFrame LessonFrame { get; set; }
        public virtual ICollection<LessonAnswerLearner> LessonAnswerLearner { get; set; }
    }
}
