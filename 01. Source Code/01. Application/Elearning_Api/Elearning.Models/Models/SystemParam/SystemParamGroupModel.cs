using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.SystemParam
{
    public class SystemParamGroupModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Index { get; set; }
        public List<SystemParamModel> ListSytemParam { get; set; }
        public SystemParamGroupModel()
        {
            ListSytemParam = new List<SystemParamModel>();
        }
    }
}
