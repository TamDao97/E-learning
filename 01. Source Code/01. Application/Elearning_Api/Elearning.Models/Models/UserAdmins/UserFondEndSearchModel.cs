using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.UserAdmins
{
    public class UserFondEndSearchModel : SearchBaseModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProvinceId { get; set; }
        public string DistrictId { get; set; }
        public string WardId { get; set; }
        public bool? IsDisable { get; set; }
        public int? NationId { get; set; }
        public int? Provider { get; set; }
        public int? Old { get; set; }

    }
}
