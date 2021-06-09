using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.HomeSillder
{
    /// <summary>
    /// 
    /// </summary>
    public class HomeSilderSearchModel: SearchBaseModel
    {

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        public bool? Status { get; set; }

    }
}
