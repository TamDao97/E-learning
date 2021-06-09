using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EmployeeSpecialist
{
    public class EmployeeSpecialistSearch : SearchBaseModel
    {

        /// <summary>
        /// Chuyên gia
        /// </summary>
        public string EmployeeName { get; set; }

    }
}
