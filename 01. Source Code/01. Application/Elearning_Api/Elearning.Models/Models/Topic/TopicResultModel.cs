using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Topic
{
   public class TopicResultModel
    {
        public string Id { get; set; }
        public string ParentTopicId { get; set; }
        public string Name { get; set; }
        public string ParentName { get; set; }
        public int TotalQuestion { get; set; }
        public List<TopicResultModel> Children { get; set; }

    }
}
