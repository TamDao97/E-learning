using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Entities
{
    public class Ward
    {
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Location { get; set; }
        public string FeatureOpenLayers { get; set; }

    }
}
