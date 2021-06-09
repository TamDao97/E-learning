using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Models.Entities
{
    public partial class LessonFrame
    {
        public LessonFrame()
        {
            LessonFrameHistory = new HashSet<LessonFrameHistory>();
            LessonFrameQuestion = new HashSet<LessonFrameQuestion>();
        }

        [Key]
        [StringLength(36)]
        public string Id { get; set; }
        [Required]
        [StringLength(36)]
        public string LessonId { get; set; }
        [Required]
        [StringLength(300)]
        public string Name { get; set; }
        public string Content { get; set; }
        public int Type { get; set; }
        public string EstimatedTime { get; set; }
        public int TestTime { get; set; }
        [Required]
        [StringLength(36)]
        public string CreateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime CreateDate { get; set; }
        [Required]
        [StringLength(36)]
        public string UpdateBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime UpdateDate { get; set; }
        public int TotalRequestCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public int DisplayIndex { get; set; }

        [InverseProperty("LessonFrame")]
        public virtual ICollection<LessonFrameHistory> LessonFrameHistory { get; set; }
        [InverseProperty("LessonFrame")]
        public virtual ICollection<LessonFrameQuestion> LessonFrameQuestion { get; set; }
    }
}
