using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Elearning.Model.Entities
{
    public class UserHistories
    {
        [Key]
        //[Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }
        public string UserId { get; set; }
        public string Content { get; set; }
        public string ClientIP { get; set; }
        public string BrowserName { get; set; }
        public string BrowserVersion { get; set; }
        public string OS { get; set; }
        public string Device { get; set; }
        public DateTime CreateDate { get; set; }
        public int Type { get; set; }

    }
}
