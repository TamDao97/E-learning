using Elearning.Model.Models.Topic;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Topics
{
    public interface ITopicService
    {
        /// <summary>
        /// Tìm kiếm chương trình đào tạo
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<TopicResultModel>> SearchTopicAsync (TopicSearchModel searchModel);
        /// <summary>
        /// Lấy thông tin chương trình đào tạo theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<TopicInfoModel> GetTopicByIdAsync (string id, string userId);
        /// <summary>
        /// Thêm chương trình đào tạo
        /// </summary>
        /// <param name="topicModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateTopicAsync (HttpRequest request, TopicModel topicModel, string userId);
        /// <summary>
        /// Cập nhập chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="topicModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateTopicAsync (HttpRequest request, string id, TopicModel topicModel, string userId);
        /// <summary>
        /// Xóa chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteTopicByIdAsync (HttpRequest request, string id, string userId);
    }
}
