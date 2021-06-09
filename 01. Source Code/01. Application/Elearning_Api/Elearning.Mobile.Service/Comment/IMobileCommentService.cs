using Elearning.Model.Models.Mobile.MobileComment;
using Elearning.Model.Models.MobileComment;
using Elearning.Models.Base;
using NTS.Common.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.Comment
{
    public interface IMobileCommentService
    {
        /// <summary>
        /// Thêm mới thông tin phản hồi theo bài giảng
        /// </summary>
        /// <param name="model">Dữ liệu thông tin phản hồi</param>
        /// <returns></returns>
        Task CreateComment(MobileCommentModel model);

        /// <summary>
        /// Danh sách phản hồi
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        Task<SearchBaseResultModel<MobileCommentResultModel>> SearchComment(MobileCommentSearchModel searchModel);
    }
}
