using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EmployeeSpecialist
{
    public class EmployeeSearchModel
    {
        public string Name { get; set; }
        public List<string> ListIdSelect { get; set; }

        public EmployeeSearchModel()
        {
            ListIdSelect = new List<string>();
        }
    }
}
