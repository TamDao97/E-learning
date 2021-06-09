using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Comment
{
    public class CommentSearchResultModel
    {
        /// <summary>
        /// Id comment
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// Id comment cha
        /// </summary>
        public Nullable<long> ParentCommentId { get; set; }
        /// <summary>
        /// Khóa học
        /// </summary>
        public string CourseId { get; set; }
        /// <summary>
        /// Bài giảng
        /// </summary>
        public string LessonId { get; set; }
        /// <summary>
        /// Đối tượng
        /// </summary>
        public string ObjectId { get; set; }
        /// <summary>
        /// Loại đối tượng
        /// </summary>
        public int ObjectType { get; set; }
        /// <summary>
        /// Nội dung
        /// </summary>
        public string Content { get; set; }
        /// <summary>
        /// Ngày comment
        /// </summary>
        public DateTime CommentDate { get; set; }
        /// <summary>
        /// Tình trạng
        /// </summary>
        public int Status { get; set; }
        /// <summary>
        /// Ảnh
        /// </summary>
        public string ImagePath { get; set; }
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string AcountName { get; set; }
        public string CategoryName { get; set; }
        public string ParentCategoryId { get; set; }
        /// <summary>
        /// Email
        /// </summary>
        public string Email { get; set; }
        /// <summary>
        /// Bài giảng
        /// </summary>
        public string LessonName { get; set; }
        /// <summary>
        /// Tổng comment của bài giảng
        /// </summary>
        public int Total { get; set; }

        public List<CommentSearchResultModel> ListReply { get; set; }
        public CommentSearchResultModel()
        {
            ListReply = new List<CommentSearchResultModel>();
        }
    }
}
