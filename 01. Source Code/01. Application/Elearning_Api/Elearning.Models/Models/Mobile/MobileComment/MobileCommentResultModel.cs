using System;
using System.Collections.Generic;
using System.Text;

namespace Elearning.Model.Models.Mobile.MobileComment
{
    public class MobileCommentResultModel
    {
        /// <summary>
        /// Id phản hồi
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Id phản hồi con
        /// </summary>
        public long? ParentCommentId { get; set; }

        /// <summary>
        /// Nội dung phản hồi
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Tên người phản hồi
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Đường dẫn ảnh
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 0: Chưa phê duyệt
        /// 1: Phê duyệt hiển thị ra bên ngoài
        /// 2: Xóa bình luận
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày tạo phản hồi
        /// </summary>
        public DateTime CommentDate { get; set; }

        public List<MobileCommentResultModel> ListReply { get; set; }
        public MobileCommentResultModel()
        {
            ListReply = new List<MobileCommentResultModel>();
        }
    }
}
