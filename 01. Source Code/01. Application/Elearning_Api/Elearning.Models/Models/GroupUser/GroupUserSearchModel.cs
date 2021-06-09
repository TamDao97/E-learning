using System;
using System.Linq;
using Elearning.Models.Base;

namespace NTS.Model.GroupUser
{
    public class GroupUserSearchModel : SearchBaseModel
    {

        public string Name { get; set; }
        public bool? IsDisable { get; set; }

    }
}