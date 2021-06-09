using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class LessonSearchResultModel
    {
        public string Id { get; set; }
        public string CategoryId { get; set; }
        public string ParentCategoryId { get; set; }
        public string CategoryName { get; set; }
        public string ManagerUnitId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string ImagePath { get; set; }
        public bool Status { get; set; }
        public int Type { get; set; }
        public bool IsExam { get; set; }
        public int? ExamTime { get; set; }
        public int? StatusApproval { get; set; }
        public DateTime CreateDate { get; set; }
        public string UpdateBy { get; set; }
        public string CreateBy { get; set; }
        public DateTime UpdateDate { get; set; }
        public string ManageUnitName { get; set; }
    }
}
