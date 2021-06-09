using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EmployeeSpecialist
{
    public class EmployeeSpecialistCreateModel
    {

        /// <summary>
        /// Ảnh chuyên gia
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Mô tả
        /// </summary>
        public string Description { get; set; }

        public List<EmployeeSpecialistCreate> ListEmployeeSpeciallist { get; set; }

        public EmployeeSpecialistCreateModel()
        {
            ListEmployeeSpeciallist = new List<EmployeeSpecialistCreate>();
        }
    }
}
