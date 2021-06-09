using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Linq;

namespace NTS.Model.GroupUser
{
    public class GroupUserResultModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public bool IsDisable { get; set; }
        public string Description { get; set; }
    }
}