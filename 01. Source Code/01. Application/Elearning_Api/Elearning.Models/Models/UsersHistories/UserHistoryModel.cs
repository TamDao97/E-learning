using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Models.UsersHistory
{
    public class UserHistoryModel
    {
        public string Id { set; get; }
        public string UserId { set; get; }
        public string Content { set; get; }
        public string ClientIP { set; get; }
        public string OS { set; get; }
        public string Device { set; get; }
        public DateTime? CreateDate { set; get; }
    }
}
