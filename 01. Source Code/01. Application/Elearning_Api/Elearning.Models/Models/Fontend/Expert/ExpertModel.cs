using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Fontend.Expert
{
    public class ExpertModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public List<EmployeeSpecialistModel> EmployeeSpecialistModel { get; set; }
    }

    public class EmployeeSpecialistModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Avatar { get; set; }
        public string Facebook { get; set; }
        public string Lotus { get; set; }
        public string Twitter { get; set; }
        public string Instagram { get; set; }
    }
}
