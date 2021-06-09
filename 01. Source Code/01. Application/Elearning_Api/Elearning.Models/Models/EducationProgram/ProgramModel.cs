using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.EducationProgram
{
    public class ProgramModel
    {
        public string Id { get; set; }
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Name, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Code, ResourceType = typeof(TextResource))]
        [MaxLength(100, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Code { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
    }
}
