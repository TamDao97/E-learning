using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Topic
{
   public class TopicModel
    {
        public string Id { get; set; }
        public string ParentTopicId { get; set; }
        public string Name { get; set; }
    }
}
