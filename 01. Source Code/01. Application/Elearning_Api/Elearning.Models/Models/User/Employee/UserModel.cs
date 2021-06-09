using Elearning.Models.GroupUser;
using System.Collections.Generic;

namespace Elearning.Models.User.Employee
{
    public class UserModel
    {
        public string Id { get; set; }
        public string ObjectId { get; set; }
        public string UserName { get; set; }
        public string SercurityStamp { get; set; }
        public string PasswordHash { get; set; }
        public bool IsLogin { get; set; }
        public bool IsDisable { get; set; }
        public int Type { get; set; }
        public List<GroupFunctionModel> ListFuntion { get; set; }

        public UserModel ()
        {
            ListFuntion = new List<GroupFunctionModel>();
        }

    }
}
