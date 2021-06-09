using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.EmployeeSpecialist
{
    public class HomeSpecialistResultModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public List<EmployeeSpecialistResultModel> employeeSpecialists { get; set; }

        public HomeSpecialistResultModel()
        {
            employeeSpecialists = new List<EmployeeSpecialistResultModel>();
        }
    }
}
