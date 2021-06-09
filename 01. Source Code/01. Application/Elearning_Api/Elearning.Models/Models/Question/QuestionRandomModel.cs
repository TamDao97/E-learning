using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.Question
{
    public class QuestionRandomModel
    {
        public int NumberQuestion { get; set; }
        public List<string> ListId { get; set; }
        public List<string> ListTopic { get; set; }
        public QuestionRandomModel()
        {
            ListId = new List<string>();
            ListTopic = new List<string>();
        }
    }
}
