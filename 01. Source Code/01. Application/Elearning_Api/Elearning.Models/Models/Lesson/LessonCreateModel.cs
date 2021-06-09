using Elearning.Model.Models.LessonFrame;
using Elearning.Model.Models.Question;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Elearning.Model.Models.Lesson
{
    public class LessonCreateModel
    {
        public string Id { get; set; }
        /// <summary>
        /// Id danh mục
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Category, ResourceType = typeof(TextResource))]
        [MaxLength(36, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string CategoryId { get; set; }
        /// <summary>
        /// Tên bài giảng
        /// </summary>
        [Required(ErrorMessageResourceName = MessageResourceKey.MSG0018, ErrorMessageResourceType = typeof(MessageResource))]
        [Display(Name = TextResourceKey.Name, ResourceType = typeof(TextResource))]
        [MaxLength(300, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Name { get; set; }
        /// <summary>
        /// Mô tả
        /// </summary>
        [Display(Name = TextResourceKey.Description, ResourceType = typeof(TextResource))]
        [MaxLength(500, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string Description { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
        [Display(Name = TextResourceKey.ImagePath, ResourceType = typeof(TextResource))]
        [MaxLength(500, ErrorMessageResourceName = MessageResourceKey.MSG0020, ErrorMessageResourceType = typeof(MessageResource))]
        public string ImagePath { get; set; }
        /// <summary>
        /// Tình trạng
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// Loại bài giảng
        /// 1: Bài giảng lý thuyết
        /// 2: Bài giảng câu hỏi trắc nghiệm
        /// 3: Bài thi
        /// </summary>
        public int Type { get; set; }
        /// <summary>
        /// Bài thi
        /// </summary>
        public bool IsExam { get; set; }
        /// <summary>
        /// Thời gian
        /// </summary>
        public int? ExamTime { get; set; }
        public string Slug { get; set; }        
        public int? ApprovalStatus { get; set; }
        public string ApprovalBy { get; set; }
        public DateTime? ApprovalDate { get; set; }
        public string RequestBy { get; set; }
        public DateTime? RequestDate { get; set; }
        public int TotalRequestCorrect { get; set; }
        public int TotalQuestion { get; set; }
        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        public List<LessonQuestionModel> ListQuestion { get; set; }
        public List<LessonFrameModel> ListLessonFrame { get; set; }
        public LessonCreateModel()
        {
            ListQuestion = new List<LessonQuestionModel>();
            ListLessonFrame = new List<LessonFrameModel>();
        }
    }
}
