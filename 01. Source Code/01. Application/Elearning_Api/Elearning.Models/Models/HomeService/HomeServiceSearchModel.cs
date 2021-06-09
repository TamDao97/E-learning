using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.HomeService
{
    public class HomeServiceSearchModel: SearchBaseModel
    {
        public bool? Status { get; set; }
        public string Title { get; set; }
    }
}
