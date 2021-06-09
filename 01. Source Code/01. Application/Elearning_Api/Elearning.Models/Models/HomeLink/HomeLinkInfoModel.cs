using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.HomeLink
{
   public class HomeLinkInfoModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string PageLink { get; set; }
        public bool Status { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
