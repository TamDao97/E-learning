using Elearning.Model.Entities;
using Elearning.Model.Models.Answer;
using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Question;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Elearning.Services.Log;
using Elearning.Services.Questions;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.Question
{
    public class QuestionService : IQuestionService
    {
        private readonly ElearningContext sqlContext;
        private readonly IDetectionService _detection;


        public QuestionService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this.sqlContext = sqlContext;
            this._detection = _detection;

        }

        /// <summary>
        /// Tìm kiếm câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseStatusModel<QuestionSearchResultModel>> SearchAsync(QuestionsSearchConditionModel searchModel, int level, string managerUnitId)
        {
            var data = (from a in sqlContext.Question.AsNoTracking()
                        join t in sqlContext.Topic.AsNoTracking() on a.TopicId equals t.Id into at
                        from ta in at.DefaultIfEmpty()
                        join u in sqlContext.User.AsNoTracking() on a.CreateBy equals u.Id into au
                        from ua in au.DefaultIfEmpty()
                        select new QuestionSearchResultModel
                        {
                            Id = a.Id,
                            TopicId = a.TopicId,
                            TopicName = ta.Name,
                            Content = a.Content,
                            Type = a.Type,
                            Status = a.Status,
                            CreateByName = ua.UserName,
                            CreateDate = a.CreateDate,
                            ManagerUnitId = ua.ManagerUnitId,
                            StatusApproval = a.ApprovalStatus
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.TopicId))
            {
                data = data.Where(i => i.TopicId.Equals(searchModel.TopicId));
            }

            if (!string.IsNullOrEmpty(searchModel.Content))
            {
                data = data.Where(i => i.Content.ToUpper().Contains(searchModel.Content.ToUpper()));
            }

            if (searchModel.Type.HasValue)
            {
                data = data.Where(i => i.Type == searchModel.Type);
            }

            if (searchModel.Status.HasValue)
            {
                data = data.Where(i => i.Status == searchModel.Status);
            }

            if (level != 1)
            {
                data = data.Where(i => i.ManagerUnitId.Equals(managerUnitId));
            }

            SearchBaseStatusModel<QuestionSearchResultModel> searchResult = new SearchBaseStatusModel<QuestionSearchResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Count();
            searchResult.TotalCreating = data.Where(i => i.StatusApproval == Constants.Course_Approval_Creating).Count();
            searchResult.TotalRequest = data.Where(i => i.StatusApproval == Constants.Course_Approval_Request).Count();
            searchResult.TotalApproval = data.Where(i => i.StatusApproval == Constants.Course_Approval_Approved).Count();
            searchResult.TotalNotApproval = data.Where(i => i.StatusApproval == Constants.Course_Approval_NotApproved).Count();
            searchResult.TotalNotBrowse = data.Where(i => i.StatusApproval == Constants.Course_Approval_NotBrowse).Count();

            if (searchModel.ApprovalStatus.HasValue)
            {
                data = data.Where(i => i.StatusApproval == searchModel.ApprovalStatus);
            }

            searchResult.DataResults = await data.OrderByDescending(a => a.CreateDate)
                .Skip((searchModel.PageNumber - 1) * searchModel.PageSize)
                .Take(searchModel.PageSize)
                .ToListAsync();

            return searchResult;
        }

        /// <summary>
        /// Tạo mới câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateAsync(HttpRequest request, QuestionCreateModel model, string userId, string manageUnit)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                try
                {
                    Elearning.Models.Entities.Question question = new Elearning.Models.Entities.Question
                    {
                        Id = Guid.NewGuid().ToString(),
                        TopicId = model.TopicId,
                        Content = model.Content,
                        ContentClear = model.Name,
                        Type = model.Type,
                        Status = model.Status.Value,
                        CreateBy = userId,
                        CreateDate = DateTime.Now,
                        UpdateBy = userId,
                        UpdateDate = DateTime.Now,
                        ManagerUnitId = manageUnit,
                    };
                    sqlContext.Question.Add(question);

                    Answer answer = new Answer();
                    List<Answer> listAnswer = new List<Answer>();

                    foreach (var item in model.ListAnswer)
                    {
                        answer = new Answer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = question.Id,
                            AnswerContent = item.AnswerContent ?? string.Empty,
                            AnswerLabel = item.AnswerLabel ?? string.Empty,
                            IsCorrect = item.IsCorrect,
                            DisplayIndex = item.DisplayIndex,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        listAnswer.Add(answer);
                    };

                    sqlContext.Answer.AddRange(listAnswer);

                    var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
                    if (user == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                    }

                    // Lưu lịch sử
                    string action = $"Tài khoản {user.UserName} đang tạo câu hỏi.";
                    ApprovalHistorys(question, "", action);

                    await sqlContext.SaveChangesAsync();
                    trans.Commit();

                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Thêm mới câu hỏi là: " + model.Name;
                    LogService.Event(userHistory, _detection);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Cập nhập câu hỏi
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateAsync(HttpRequest request, QuestionCreateModel model, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                try
                {
                    Elearning.Models.Entities.Question question = null;
                    question = sqlContext.Question.Find(model.Id);
                    string NameOld = question.Content;
                    if (question == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
                    }

                    question.TopicId = model.TopicId;
                    question.Content = model.Content;
                    question.ContentClear = model.Name;
                    question.Type = model.Type;
                    question.Status = model.Status.Value;
                    question.UpdateBy = userId;
                    question.UpdateDate = DateTime.Now;

                    Answer answer = null;
                    List<Answer> listAnswer = new List<Answer>();
                    listAnswer = sqlContext.Answer.Where(r => r.QuestionId.Equals(model.Id)).ToList();
                    sqlContext.Answer.RemoveRange(listAnswer);

                    listAnswer = new List<Answer>();
                    foreach (var item in model.ListAnswer)
                    {
                        answer = new Answer()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = model.Id,
                            AnswerContent = item.AnswerContent ?? string.Empty,
                            AnswerLabel = item.AnswerLabel ?? string.Empty,
                            IsCorrect = item.IsCorrect,
                            DisplayIndex = item.DisplayIndex,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        listAnswer.Add(answer);
                    };

                    // Lưu lịch sử
                    var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
                    if (user == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                    }
                    string action = $"Tài khoản {user.UserName} đã cập nhật câu hỏi: {model.Name}";
                    ApprovalHistorys(question, "", action);

                    sqlContext.Answer.AddRange(listAnswer);

                    await sqlContext.SaveChangesAsync();
                    trans.Commit();

                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);

                    userHistory.Content = "Cập nhật câu hỏi là: " + model.Name;
                    LogService.Event(userHistory, _detection);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Xóa câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteAsync(HttpRequest request, string id, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                try
                {
                    var question = sqlContext.Question.Find(id);

                    if (question == null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
                    }

                    var lessonQuestion = sqlContext.LessonQuestion.FirstOrDefault(i => i.QuestionId.Equals(id));
                    if (lessonQuestion != null)
                    {
                        throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Question);
                    }

                    //if (question.Status.Equals(Constants.Question_Status_Active))
                    //{
                    //    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Question);
                    //}

                    var answer = sqlContext.Answer.Where(r => r.QuestionId.Equals(id));

                    sqlContext.Remove(question);

                    if (answer != null)
                    {
                        sqlContext.RemoveRange(answer);
                    }

                    var questionApprovalHistories = sqlContext.QuestionApprovalHistory.Where(i => i.QuestionId.Equals(id)).ToList();
                    if (questionApprovalHistories.Count > 0)
                    {
                        sqlContext.QuestionApprovalHistory.RemoveRange(questionApprovalHistories);
                    }

                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Xóa câu hỏi: " + question.ContentClear;

                    await sqlContext.SaveChangesAsync();
                    trans.Commit();


                    LogService.Event(userHistory, _detection);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<QuestionCreateModel> GetQuestionByIdAsync(string id)
        {
            var question = await sqlContext.Question.FindAsync(id);

            if (question == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }

            List<AnswerInfoModel> listAnswer = sqlContext.Answer.Where(r => r.QuestionId.Equals(id)).Select(r => new AnswerInfoModel
            {
                Id = r.Id,
                QuestionId = r.QuestionId,
                AnswerContent = r.AnswerContent,
                AnswerLabel = r.AnswerLabel,
                IsCorrect = r.IsCorrect,
                DisplayIndex = r.DisplayIndex
            }).OrderBy(r => r.AnswerLabel).ToList();

            QuestionCreateModel result = new QuestionCreateModel
            {
                Id = question.Id,
                TopicId = question.TopicId,
                Content = question.Content,
                Type = question.Type,
                Status = question.Status,
                ApprovalStatus = question.ApprovalStatus,
                ListAnswer = listAnswer
            };

            return result;
        }

        public async Task UpdateStatusQuestionsync(HttpRequest request, string id, string userId)
        {
            var courseExist = sqlContext.Question.Find(id);
            if (courseExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }
            if (courseExist.Status == true)
            {
                courseExist.Status = false;
            }
            else
            {
                courseExist.Status = true;
            }

            var statusName = this.getNameStatus(courseExist.Status);

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái câu hỏi : " + courseExist.Content + "thành " + statusName;

            sqlContext.SaveChanges();


            LogService.Event(userHistory, _detection);
        }


        public string getNameStatus(bool status)
        {
            if (status == true)
            {
                return "Hiển thị";
            }
            else
                return "Không hiển thị";
        }

        /// <summary>
        /// Yêu cầu duyệt câu hỏi
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RequestQuestionAsync(string id, string userId, StatusModel model)
        {
            var question = sqlContext.Question.FirstOrDefault(i => i.Id.Equals(id));
            if (question == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }

            var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = sqlContext.Database.BeginTransaction())
            {
                question.ApprovalStatus = Constants.Course_Approval_Request;
                question.RequestBy = userId;
                question.RequestDate = DateTime.Now;

                // Lưu lịch sử
                string content = $"Tài khoản {user.UserName} đã yêu cầu duyệt.";
                ApprovalHistorys(question, model.Content, content);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task ApprovalQuestionAsync(string id, string userId, StatusModel model)
        {
            var question = sqlContext.Question.FirstOrDefault(i => i.Id.Equals(id));
            if (question == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Question);
            }

            var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = sqlContext.Database.BeginTransaction())
            {
                question.ApprovalStatus = model.Status;
                question.ApprovalBy = userId;
                question.ApprovalDate = DateTime.Now;

                // Lưu lịch sử
                string action = string.Empty;
                if (model.Status == Constants.Course_Approval_Approved)
                {
                    action = $"Tài khoản {user.UserName} đã duyệt.";
                }
                else if (model.Status == Constants.Course_Approval_Creating)
                {
                    action = $"Tài khoản {user.UserName} đang tạo.";
                }
                else if (model.Status == Constants.Course_Approval_NotApproved)
                {
                    action = $"Tài khoản {user.UserName} hủy duyệt.";
                }
                else if (model.Status == Constants.Course_Approval_NotBrowse)
                {
                    action = $"Tài khoản {user.UserName} không duyệt.";
                }
                ApprovalHistorys(question, model.Content, action);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void ApprovalHistorys(Elearning.Models.Entities.Question question, string content, string action)
        {
            QuestionApprovalHistory approvalHistory = new QuestionApprovalHistory()
            {
                QuestionId = question.Id,
                Action = action,
                Content = content,
                ApprovalStatus = question.ApprovalStatus,
                ProcessingDate = DateTime.Now
            };

            sqlContext.QuestionApprovalHistory.Add(approvalHistory);
        }

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<QuestionApprovalHistoryModel>> GetListQuestionApprovalStatusAsync(string id)
        {
            var data = await (from a in sqlContext.QuestionApprovalHistory.AsNoTracking()
                              where a.QuestionId.Equals(id)
                              orderby a.ProcessingDate descending
                              select new QuestionApprovalHistoryModel
                              {
                                  Id = a.Id,
                                  QuestionId = a.QuestionId,
                                  Action = a.Action,
                                  Content = a.Content,
                                  ApprovalStatus = a.ApprovalStatus,
                                  ProcessingDate = a.ProcessingDate
                              }).ToListAsync();

            return data;
        }
    }
}
