using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.HomeLink
{
    public class HomeLinkModel
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Title, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Title { get; set; }
        public string Description { get; set; }
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.PageLink, ResourceType = typeof(TextResource))]
        [MaxLength(500, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string PageLink { get; set; }
        [Required]
        public bool Status { get; set; }
    }
}
