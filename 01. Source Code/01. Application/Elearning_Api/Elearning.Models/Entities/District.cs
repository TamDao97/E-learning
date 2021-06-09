using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    public class District
    {
        public string DistrictId { get; set; }
        public string ProvinceId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string FeatureOpenLayers { get; set; }
    }
}
