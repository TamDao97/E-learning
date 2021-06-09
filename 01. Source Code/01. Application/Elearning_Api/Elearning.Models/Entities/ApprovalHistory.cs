using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Elearning.Model.Entities
{
    public partial class ApprovalHistory
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(36)]
        public string CourseId { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ProcessingDate { get; set; }
        public int ApprovalStatus { get; set; }
        public virtual Course Course { get; set; }
    }
}
