using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Entities
{
    public partial class HomeSetting
    {
        [Key]
        public int Id { get; set; }
        public string Logo { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public string Gmail { get; set; }
        public string Website { get; set; }
        public string LinkYoutube { get; set; }
        public string LinkFacebook { get; set; }
        public string LinkGoogle { get; set; }
        public string Copyright { get; set; }
        public string CreateBy { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public DateTime UpdateDate { get; set; }
    }
}
