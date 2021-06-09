using Elearning.Models.GroupUser;
using System.Collections.Generic;

namespace Elearning.Models.User.Employee
{
    public class UserPermissionModel
    {
        /// <summary>
        /// Id quyền user
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// id tài khoản
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Id quyền
        /// </summary>
        public string PermissionId { get; set; }

        /// <summary>
        /// đếm
        /// </summary>
        public int dem { get; set; }

        /// <summary>
        /// tên
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// List chức năng
        /// </summary>
        public List<PermissionModel> ListFunctionParameter { set; get; }

        /// <summary>
        /// List giá trị
        /// </summary>
        public List<PermissionModel> ListValue { set; get; }

        public UserPermissionModel ()
        {
            ListValue = new List<PermissionModel>();
        }
    }
}