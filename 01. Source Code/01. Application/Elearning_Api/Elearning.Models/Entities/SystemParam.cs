using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public class SystemParam
    {
        [Key]
        public string SystemParamId { get; set; }

        [Required]
        [MaxLength(300)]
        public string ParamName { get; set; }

        public string ParamValue { get; set; }
        public string DisplayName { get; set; }
        public int Index { get; set; }
        public int ControlType { get; set; }
        public string SystemParamGroupId { get; set; }
    }
}
