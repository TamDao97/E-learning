using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Model.Entities
{
    public partial class Test
    {
        public Test()
        {
            AnswerLearner = new HashSet<AnswerLearner>();
        }

        [Key]
        public string Id { get; set; }
        [Required]
        [StringLength(36)]
        public string LearnerId { get; set; }
        [Required]
        [StringLength(36)]
        public string CourseId { get; set; }
        [Required]
        [StringLength(36)]
        public string LessonId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime StartDate { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? FinishDate { get; set; }
        public int TotalQuestion { get; set; }
        public int TotalCorrect { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual Course Course { get; set; }
        public virtual ICollection<AnswerLearner> AnswerLearner { get; set; }
    }
}
