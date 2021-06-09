using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.HomeService
{
    public class HomeServiceResultModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public int DisplayIndex { get; set; }
    }
}
