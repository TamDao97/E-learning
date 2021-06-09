using Elearning.Model.Models.Topic;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.Topics
{
    public class TopicService : ITopicService
    {
        private readonly IDetectionService _detection;
        private readonly ElearningContext _sqlContext;

        public TopicService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this._detection = _detection;
            this._sqlContext = sqlContext;
        }
        public async Task CreateTopicAsync(HttpRequest request, TopicModel topicModel, string userId)
        {
            var checkProgram = _sqlContext.Topic.FirstOrDefault(u => u.Name.ToLower().Equals(topicModel.Name.ToLower()));
            if (checkProgram != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Topic);
            }
            Elearning.Models.Entities.Topic topic = new Elearning.Models.Entities.Topic()
            {
                Id = Guid.NewGuid().ToString(),
                Name = topicModel.Name,
                ParentTopicId = topicModel.ParentTopicId,
                CreateDate = DateTime.Now,
                CreateBy = userId,
                UpdateBy = userId,
                UpdateDate = DateTime.Now,
            };
            _sqlContext.Topic.Add(topic);
            _sqlContext.SaveChanges();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Thêm mới chủ đề: " + topicModel.Name;
            LogService.Event(userHistory, _detection);
        }

        public async Task DeleteTopicByIdAsync(HttpRequest request, string id, string userId)
        {
            var list = await _sqlContext.Topic.ToListAsync();
            var topicExist = _sqlContext.Topic.Find(id);
            if (topicExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Topic);
            }
            var checkQuestion = _sqlContext.Question.Where(s => s.TopicId == id).Count();
            if (checkQuestion > 0)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Topic);
            }
            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa chủ đề: " + topicExist.Name;

            var data = GetListTopicChild(id, list);
            if (data.Count > 0)
            {
                var check = _sqlContext.Question.FirstOrDefault(i => data.Select(a => a.Id).Contains(i.TopicId));
                if (check != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.TopicChild);
                }
            }

            _sqlContext.Topic.RemoveRange(data);
            _sqlContext.Topic.Remove(topicExist);
            _sqlContext.SaveChanges();

            LogService.Event(userHistory, _detection);
        }

        public List<Topic> GetListTopicChild(string parentId, List<Topic> list)
        {
            List<Topic> topics = new List<Topic>();

            var data = list.Where(i => !string.IsNullOrEmpty(i.ParentTopicId) && i.ParentTopicId.Equals(parentId));
            topics.AddRange(data);
            foreach (var item in data)
            {
                topics.AddRange(GetListTopicChild(item.Id, list));
            }

            return topics;
        }

        public async Task<TopicInfoModel> GetTopicByIdAsync(string id, string userId)
        {
            var topicInfo = (from a in _sqlContext.Topic.AsNoTracking()
                             where a.Id.Equals(id)
                             select new TopicInfoModel
                             {
                                 Id = a.Id,
                                 Name = a.Name,
                                 ParentTopicId = a.ParentTopicId,
                             }).FirstOrDefault();

            if (topicInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Topic);
            }
            return topicInfo;
        }

        public async Task<SearchBaseResultModel<TopicResultModel>> SearchTopicAsync(TopicSearchModel searchModel)
        {
            var topics = (from a in _sqlContext.Topic.AsNoTracking()
                          join b in _sqlContext.Topic on a.ParentTopicId equals b.Id into ab
                          from ba in ab.DefaultIfEmpty()
                          select new TopicResultModel
                          {
                              Id = a.Id,
                              Name = a.Name,
                              ParentTopicId = a.ParentTopicId,
                              ParentName = ba.Name,
                              TotalQuestion = _sqlContext.Question.Where(s => s.TopicId == a.Id).Count(),

                          }).ToList();
            var data = new List<TopicResultModel>();
            foreach (var item in topics.Where(s => s.ParentTopicId == null))
            {
                int value = 0;
                var topic = new TopicResultModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentTopicId = item.ParentTopicId,
                    ParentName = item.ParentName,
                    TotalQuestion = item.TotalQuestion + GetTotal(topics, item.Id, ref value),
                    Children = GetChild(topics, item.Id, ref value)
                };
                data.Add(topic);
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper())).ToList();
            }
            SearchBaseResultModel<TopicResultModel> searchResult = new SearchBaseResultModel<TopicResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.OrderBy(s => s.ParentName).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            return searchResult;
        }

        //private getchild (List<string> all, string parentId)
        //{
        //    var childs = all.Where(r => r.Equals(parentId)).ToList();

        //    foreach (var item in childs)
        //    {
        //        getchild(all, item.Id);
        //    }
        //}

        public int GetTotal(List<TopicResultModel> topics, string id, ref int result)
        {
            var toppicchild = topics.Where(s => s.ParentTopicId == id).ToList();
            foreach (var item in toppicchild)
            {
                int totalQuestion = _sqlContext.Question.Where(s => s.TopicId == item.Id).Count();
                result = totalQuestion + GetTotal(toppicchild, item.Id, ref result);
            }
            return result;
        }

        public List<TopicResultModel> GetChild(List<TopicResultModel> topics, string id, ref int value)
        {
            var data = new List<TopicResultModel>();
            var toppicchild = topics.Where(s => s.ParentTopicId == id).ToList();
            foreach (var item in toppicchild)
            {
                TopicResultModel topic;
                topic = new TopicResultModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    ParentTopicId = item.ParentTopicId,
                    ParentName = item.ParentName,
                    TotalQuestion = item.TotalQuestion + GetTotal(toppicchild, item.Id, ref value),
                    Children = GetChild(toppicchild, item.Id, ref value)
                };
                data.Add(topic);
            }
            return data;
        }

        public async Task UpdateTopicAsync(HttpRequest request, string id, TopicModel topicModel, string userId)
        {
            var topic = await _sqlContext.Topic.FindAsync(id);
            string NameOld = topic.Name;
            if (topic == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Topic);
            }
            var topicExist = _sqlContext.Topic.AsNoTracking().FirstOrDefault(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(topicModel.Name.ToLower()));
            if (topicExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Topic);
            }

            topic.Name = topicModel.Name;
            topic.ParentTopicId = topicModel.ParentTopicId;
            topic.UpdateBy = userId;
            topic.UpdateDate = DateTime.Now;
            _sqlContext.SaveChanges();


            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

            if (NameOld.ToLower() == topicModel.Name.ToLower())
            {
                userHistory.Content = "Cập nhật chủ đề tên là: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật chủ đề có tên ban đầu là: " + NameOld + " thành " + topicModel.Name;
            }
            LogService.Event(userHistory, _detection);
        }
    }
}
