using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.SystemParam
{
    public class SystemParamModel
    {
        public string SystemParamId { get; set; }
        public string ParamName { get; set; }
        public string ParamValue { get; set; }
        public string DisplayName { get; set; }
        public int Index { get; set; }
        public int ControlType { get; set; }
    }
}
