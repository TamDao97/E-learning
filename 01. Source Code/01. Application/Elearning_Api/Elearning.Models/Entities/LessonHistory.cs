using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class LessonHistory
    {
        [Key]
        public long Id { get; set; }
        public string LessonId { get; set; }
        public string LearnerId { get; set; }
        public string CourseId { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual Course Course { get; set; }
    }
}
