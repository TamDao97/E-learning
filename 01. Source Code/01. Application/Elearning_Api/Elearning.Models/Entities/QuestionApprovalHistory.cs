using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Elearning.Model.Entities
{
    public class QuestionApprovalHistory
    {
        [Key]
        public long Id { get; set; }
        [Required]
        [StringLength(36)]
        public string QuestionId { get; set; }
        public string Action { get; set; }
        public string Content { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime ProcessingDate { get; set; }
        public int ApprovalStatus { get; set; }
        public virtual Question Question { get; set; }
    }
}
