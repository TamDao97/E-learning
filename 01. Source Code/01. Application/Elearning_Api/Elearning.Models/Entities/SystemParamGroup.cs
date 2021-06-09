using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public class SystemParamGroup
    {
        [Key]
        public string Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Name { get; set; }
        public int Index { get; set; }
    }
}
