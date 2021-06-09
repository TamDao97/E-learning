using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class Mark
    {
        [Key]
        public string Id { get; set; }
        public string LearnerId { get; set; }
        public string LessonCourseId { get; set; }
        public int TotalCorrect { get; set; }
        public int TotalQuestion { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? FinishDate { get; set; }
        public virtual Learner Learner { get; set; }
        public virtual LessonCourse LessonCourse { get; set; }

    }
}
