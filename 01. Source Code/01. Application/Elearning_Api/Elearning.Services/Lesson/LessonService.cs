
using Elearning.Model.Entities;
using Elearning.Model.Models.Answer;
using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Lesson;
using Elearning.Model.Models.LessonFrame;
using Elearning.Model.Models.Question;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Base;
using Elearning.Models.Combobox;
using Elearning.Models.Entities;
using Elearning.Services.Combobox;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Helpers;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.Lessons
{
    public class LessonService : ILessonService
    {
        private readonly ElearningContext sqlContext;
        private readonly IComboboxService comboboxService;
        private readonly IDetectionService _detection;


        public LessonService(ElearningContext sqlContext, IComboboxService comboboxService, IDetectionService _detection)
        {
            this.sqlContext = sqlContext;
            this.comboboxService = comboboxService;
            this._detection = _detection;

        }

        /// <summary>
        /// Tìm kiếm bài giảng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseStatusModel<LessonSearchResultModel>> SearchLessonAsync(LessonSearchConditionModel searchModel, int level, string managerUnitId)
        {
            var data = (from a in sqlContext.Lesson.AsNoTracking()
                        join b in sqlContext.Category.AsNoTracking() on a.CategoryId equals b.Id
                        join c in sqlContext.User.AsNoTracking() on a.CreateBy equals c.Id
                        join d in sqlContext.ManagerUnit.AsNoTracking() on a.ManagerUnitId equals d.Id
                        select new LessonSearchResultModel
                        {
                            Id = a.Id,
                            CategoryId = a.CategoryId,
                            CategoryName = b.Name,
                            ParentCategoryId = b.ParentCategoryId,
                            Name = a.Name,
                            Description = a.Description,
                            Content = a.Content,
                            ImagePath = a.ImagePath,
                            Status = a.Status,
                            Type = a.Type,
                            IsExam = a.IsExam,
                            ExamTime = a.ExamTime,
                            ManagerUnitId = c.ManagerUnitId,
                            CreateBy = c.UserName,
                            CreateDate = a.CreateDate,
                            UpdateBy = c.UserName,
                            UpdateDate = a.UpdateDate,
                            ManageUnitName = d.Name,
                            StatusApproval = a.ApprovalStatus
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(i => i.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.CategoryId))
            {
                data = data.Where(i => i.CategoryId.Equals(searchModel.CategoryId));
            }

            if (searchModel.Status.HasValue)
            {
                data = data.Where(i => i.Status == searchModel.Status);
            }

            if (searchModel.Type.HasValue)
            {
                data = data.Where(i => i.Type == searchModel.Type);
            }

            if (level != 1)
            {
                data = data.Where(i => i.ManagerUnitId.Equals(managerUnitId));
            }

            if (!string.IsNullOrEmpty(searchModel.ManageUnitId))
            {
                data = data.Where(i => i.ManagerUnitId.Equals(searchModel.ManageUnitId));
            }

            if (!string.IsNullOrEmpty(searchModel.CreateBy))
            {
                data = data.Where(i => i.CreateBy.ToUpper().Contains(searchModel.CreateBy.ToUpper()));
            }

            SearchBaseStatusModel<LessonSearchResultModel> searchResult = new SearchBaseStatusModel<LessonSearchResultModel>();
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
            searchResult.DataResults = await data.OrderByDescending(s => s.CreateDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();

            var list = await comboboxService.GetCategoryAsync();
            ComboboxParentModel comboboxParent;
            foreach (var item in searchResult.DataResults)
            {
                comboboxParent = new ComboboxParentModel();
                if (!string.IsNullOrEmpty(item.ParentCategoryId))
                {
                    item.CategoryName = GetCategoryName(item.CategoryName, item.ParentCategoryId, list);
                }
            }

            return searchResult;
        }

        public string GetCategoryName(string name, string parentId, List<ComboboxParentModel> listCategory)
        {
            string category = name;

            var data = listCategory.FirstOrDefault(i => i.Id.Equals(parentId));
            if (data != null)
            {
                category = $"{data.Name}/ {category}";
                if (!string.IsNullOrEmpty(data.ParentId))
                {
                    category = GetCategoryName(category, data.ParentId, listCategory);
                }
            }

            return category;
        }

        /// <summary>
        /// Thêm bài giảng
        /// </summary>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task CreateLessonAsync(HttpRequest request, LessonCreateModel model, string userId, string manageUnit)
        {
            var checkNameLesson = sqlContext.Lesson.FirstOrDefault(u => u.Name.ToLower().Equals(model.Name.ToLower()));

            using (var trans = sqlContext.Database.BeginTransaction())
            {
                Lesson lesson = new Lesson()
                {
                    Id = Guid.NewGuid().ToString(),
                    CategoryId = model.CategoryId,
                    Name = model.Name,
                    Description = model.Description,
                    Content = model.Content,
                    ImagePath = model.ImagePath,
                    Status = model.Status,
                    Type = model.Type,
                    IsExam = model.IsExam,
                    ExamTime = model.ExamTime,
                    Slug = checkNameLesson == null ? SlugHelper.ConverNameToSlug(model.Name) : SlugHelper.ConverNameToSlug(model.Name) + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now,
                    ManagerUnitId = manageUnit,
                    TotalRequestCorrect = model.TotalRequestCorrect,
                    TotalQuestion = model.ListQuestion.Count
                };
                sqlContext.Lesson.Add(lesson);

                if (model.Type == Constants.Lesson_Type_Study || model.Type == Constants.Lesson_Type_Exam)
                {
                    LessonQuestion lessonQuestion;
                    List<LessonQuestion> list = new List<LessonQuestion>();
                    foreach (var item in model.ListQuestion)
                    {
                        lessonQuestion = new LessonQuestion()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = item.Id,
                            LessonId = lesson.Id
                        };
                        list.Add(lessonQuestion);
                    }
                    sqlContext.LessonQuestion.AddRange(list);
                }
                else if (model.Type == Constants.Lesson_Type_Theory)
                {
                    LessonFrame lessonFrame;
                    LessonFrameQuestion lessonFrameQuestion;
                    List<LessonFrame> listLessonFrame = new List<LessonFrame>();
                    List<LessonFrameQuestion> listLessonFrameQuestion = new List<LessonFrameQuestion>();
                    int i = 1;
                    foreach (var item in model.ListLessonFrame)
                    {
                        lessonFrame = new LessonFrame()
                        {
                            Id = Guid.NewGuid().ToString(),
                            LessonId = lesson.Id,
                            Name = item.Name,
                            Content = item.Content,
                            Type = item.Type,
                            EstimatedTime = item.EstimatedTime,
                            TestTime = item.TestTime,
                            DisplayIndex = i++,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        if (item.Type == Constants.Lesson_Type_Study)
                        {
                            foreach (var itemQuestion in item.ListQuestion)
                            {
                                lessonFrameQuestion = new LessonFrameQuestion()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    LessonFrameId = lessonFrame.Id,
                                    QuestionId = itemQuestion.Id
                                };
                                listLessonFrameQuestion.Add(lessonFrameQuestion);
                            }
                            lessonFrame.TotalQuestion = item.ListQuestion.Count;
                            lessonFrame.TotalRequestCorrect = item.TotalRequestCorrect.Value;
                        }
                        listLessonFrame.Add(lessonFrame);
                    }
                    sqlContext.LessonFrame.AddRange(listLessonFrame);
                    sqlContext.LessonFrameQuestion.AddRange(listLessonFrameQuestion);
                }

                var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
                if (user == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                }

                // Lưu lịch sử
                string action = $"Tài khoản {user.UserName} đang tạo bài giảng: {lesson.Name}";
                ApprovalHistorys(lesson, "", action);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Thêm mới bài giảng: " + model.Name;
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
        /// Cập nhật bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task UpdateLessonAsync(HttpRequest request, string id, LessonCreateModel model, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                var lesson = await sqlContext.Lesson.FirstOrDefaultAsync(i => i.Id.Equals(id));
                string NameOld = lesson.Name;
                if (lesson == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
                }

                var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
                if (user == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                }

                var checkNameLesson = await sqlContext.Lesson.FirstOrDefaultAsync(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower()));
                lesson.CategoryId = model.CategoryId;
                lesson.Name = model.Name;
                lesson.Description = model.Description;
                lesson.Content = model.Content;
                lesson.ImagePath = model.ImagePath;
                lesson.Status = model.Status;
                lesson.Type = model.Type;
                lesson.Slug = checkNameLesson == null ? SlugHelper.ConverNameToSlug(model.Name) : SlugHelper.ConverNameToSlug(model.Name) + DateTime.Now.ToString("yyyyMMddHHmmss");
                lesson.IsExam = model.IsExam;
                lesson.ExamTime = model.ExamTime;
                lesson.TotalRequestCorrect = model.TotalRequestCorrect;
                lesson.TotalQuestion = model.ListQuestion.Count;
                lesson.UpdateBy = userId;
                lesson.UpdateDate = DateTime.Now;

                var lessonQuestions = sqlContext.LessonQuestion.Where(i => i.LessonId.Equals(id)).ToList();
                if (lessonQuestions.Count > 0)
                {
                    sqlContext.RemoveRange(lessonQuestions);
                }

                var lessonFrames = sqlContext.LessonFrame.Where(i => i.LessonId.Equals(id)).ToList();
                List<LessonFrameQuestion> lessonFrameQuestions = new List<LessonFrameQuestion>();
                List<LessonFrameHistory> lessonFrameHistories = new List<LessonFrameHistory>();
                List<LessonAnswerLearner> lessonAnswerLearners = new List<LessonAnswerLearner>();

                List<LessonFrameHistory> listRemove;
                foreach (var item in lessonFrames)
                {
                    lessonFrameQuestions.AddRange(sqlContext.LessonFrameQuestion.Where(i => i.LessonFrameId.Equals(item.Id)));

                    listRemove = new List<LessonFrameHistory>();
                    listRemove = sqlContext.LessonFrameHistory.Where(i => i.LessonFrameId.Equals(item.Id)).ToList();
                    lessonFrameHistories.AddRange(listRemove);
                    foreach (var ite in listRemove)
                    {
                        lessonAnswerLearners.AddRange(sqlContext.LessonAnswerLearner.Where(i => i.LessonFrameHistoryId.Equals(ite.Id)));
                    }
                }
                sqlContext.LessonAnswerLearner.RemoveRange(lessonAnswerLearners);
                sqlContext.LessonFrameHistory.RemoveRange(lessonFrameHistories);
                sqlContext.LessonFrame.RemoveRange(lessonFrames);
                sqlContext.LessonFrameQuestion.RemoveRange(lessonFrameQuestions);

                if (model.Type == Constants.Lesson_Type_Study || model.Type == Constants.Lesson_Type_Exam)
                {
                    LessonQuestion lessonQuestion;
                    List<LessonQuestion> list = new List<LessonQuestion>();
                    foreach (var item in model.ListQuestion)
                    {
                        lessonQuestion = new LessonQuestion()
                        {
                            Id = Guid.NewGuid().ToString(),
                            QuestionId = item.Id,
                            LessonId = lesson.Id
                        };
                        list.Add(lessonQuestion);
                    }
                    sqlContext.LessonQuestion.AddRange(list);
                }
                else if (model.Type == Constants.Lesson_Type_Theory)
                {
                    LessonFrame lessonFrame;
                    LessonFrameQuestion lessonFrameQuestion;
                    List<LessonFrame> listLessonFrame = new List<LessonFrame>();
                    List<LessonFrameQuestion> listLessonFrameQuestion = new List<LessonFrameQuestion>();
                    int i = 1;
                    foreach (var item in model.ListLessonFrame)
                    {
                        lessonFrame = new LessonFrame()
                        {
                            Id = Guid.NewGuid().ToString(),
                            LessonId = lesson.Id,
                            Name = item.Name,
                            Content = item.Content,
                            Type = item.Type,
                            EstimatedTime = item.EstimatedTime,
                            TestTime = item.TestTime,
                            DisplayIndex = i++,
                            CreateBy = userId,
                            CreateDate = DateTime.Now,
                            UpdateBy = userId,
                            UpdateDate = DateTime.Now
                        };

                        if (item.Type == Constants.Lesson_Type_Study)
                        {
                            foreach (var itemQuestion in item.ListQuestion)
                            {
                                lessonFrameQuestion = new LessonFrameQuestion()
                                {
                                    Id = Guid.NewGuid().ToString(),
                                    LessonFrameId = lessonFrame.Id,
                                    QuestionId = itemQuestion.Id
                                };
                                listLessonFrameQuestion.Add(lessonFrameQuestion);
                            }
                            lessonFrame.TotalQuestion = item.ListQuestion.Count;
                            lessonFrame.TotalRequestCorrect = item.TotalRequestCorrect.Value;
                        }
                        listLessonFrame.Add(lessonFrame);
                    }
                    sqlContext.LessonFrame.AddRange(listLessonFrame);
                    sqlContext.LessonFrameQuestion.AddRange(listLessonFrameQuestion);
                }

                try
                {
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    string action = string.Empty;

                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        userHistory.Content = "Cập nhật bài giảng tên là: " + NameOld;
                        action = $"Tài khoản {user.UserName} đã cập nhật khóa học: {NameOld}";
                    }
                    else
                    {
                        userHistory.Content = "Cập nhật bài giảng có tên ban đầu là: " + NameOld + " thành " + model.Name;
                        action = $"Tài khoản {user.UserName} đã cập nhật khóa học: {NameOld} thành {model.Name}";
                    }

                    // Lưu lịch sử
                    ApprovalHistorys(lesson, "", action);

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

        /// <summary>
        /// Thay đổi trạng thái
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task UpdateStatus(HttpRequest request, string id, string userId)
        {
            var lesson = await sqlContext.Lesson.FirstOrDefaultAsync(i => i.Id.Equals(id));
            if (lesson == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (lesson.Status == true)
            {
                lesson.Status = false;
            }
            else
            {
                lesson.Status = true;
            }

            sqlContext.SaveChanges();
            var statusName = this.getNameStatus(lesson.Status);


            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái bài giảng: " + lesson.Name + " thành " + statusName;
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
        /// Lấy thông tin bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LessonCreateModel> GetLessonByIdAsync(string id, string userId)
        {
            var resultInfo = await (from a in sqlContext.Lesson.AsNoTracking()
                                    where a.Id.Equals(id)
                                    select new LessonCreateModel()
                                    {
                                        Id = a.Id,
                                        CategoryId = a.CategoryId,
                                        Name = a.Name,
                                        Description = a.Description,
                                        Content = a.Content,
                                        ImagePath = a.ImagePath,
                                        Status = a.Status,
                                        Type = a.Type,
                                        IsExam = a.IsExam,
                                        ExamTime = a.ExamTime,
                                        ApprovalStatus = a.ApprovalStatus,
                                        TotalRequestCorrect = a.TotalRequestCorrect,
                                        TotalQuestion = a.TotalQuestion
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (resultInfo.Type == Constants.Lesson_Type_Study || resultInfo.Type == Constants.Lesson_Type_Exam)
            {
                var question = (from a in sqlContext.LessonQuestion.AsNoTracking()
                                join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                join c in sqlContext.Topic.AsNoTracking() on b.TopicId equals c.Id
                                where a.LessonId.Equals(resultInfo.Id) && b.Status == Constants.Question_Status_Active
                                orderby c.Name
                                select new LessonQuestionModel
                                {
                                    Id = b.Id,
                                    TopicId = b.TopicId,
                                    TopicName = c.Name,
                                    Content = b.Content,
                                    Type = b.Type
                                }).ToList();

                resultInfo.ListQuestion = question;
            }
            else if (resultInfo.Type == Constants.Lesson_Type_Theory)
            {
                var list = (from a in sqlContext.LessonFrame.AsNoTracking()
                            where a.LessonId.Equals(resultInfo.Id)
                            orderby a.DisplayIndex
                            select new LessonFrameModel
                            {
                                Id = a.Id,
                                LessonId = a.LessonId,
                                Name = a.Name,
                                Content = a.Content,
                                Type = a.Type,
                                EstimatedTime = a.EstimatedTime,
                                TestTime = a.TestTime,
                                TotalQuestion = a.TotalQuestion,
                                TotalRequestCorrect = a.TotalRequestCorrect
                            }).ToList();

                foreach (var item in list)
                {
                    if (item.Type == Constants.Lesson_Type_Study)
                    {
                        var question = (from a in sqlContext.LessonFrameQuestion.AsNoTracking()
                                        join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                        join c in sqlContext.Topic.AsNoTracking() on b.TopicId equals c.Id
                                        where a.LessonFrameId.Equals(item.Id) && b.Status == Constants.Question_Status_Active
                                        orderby c.Name
                                        select new LessonQuestionModel
                                        {
                                            Id = b.Id,
                                            TopicId = b.TopicId,
                                            TopicName = c.Name,
                                            Content = b.Content,
                                            Type = b.Type
                                        }).ToList();
                        item.ListQuestion = question;

                        foreach (var ite in question)
                        {
                            ite.ListAnswer = (from a in sqlContext.Answer.AsNoTracking()
                                              where a.QuestionId.Equals(ite.Id)
                                              orderby a.AnswerLabel
                                              select new AnswerInfoModel
                                              {
                                                  Id = a.Id,
                                                  QuestionId = a.QuestionId,
                                                  AnswerLabel = a.AnswerLabel,
                                                  IsCorrect = false,
                                                  AnswerContent = ite.Type == 4 ? "" : a.AnswerContent,
                                                  //DisplayIndex = a.DisplayIndex
                                                  Type = item.Type,
                                              }).ToList();
                        }
                    }
                }

                resultInfo.ListLessonFrame = list;
            }

            return resultInfo;
        }

        /// <summary>
        /// Xóa bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteLessonByIdAsync(HttpRequest request, string id, string userId)
        {
            var lessonExist = await sqlContext.Lesson.FirstOrDefaultAsync(i => i.Id.Equals(id));
            if (lessonExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            var lessonCourse = sqlContext.LessonCourse.FirstOrDefault(i => i.LessonId.Equals(id));
            if (lessonCourse != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Lesson);
            }

            var lessonQuestions = sqlContext.LessonQuestion.Where(i => i.LessonId.Equals(id)).ToList();
            if (lessonQuestions.Count > 0)
            {
                sqlContext.LessonQuestion.RemoveRange(lessonQuestions);
            }

            var comments = sqlContext.Comment.Where(i => i.LessonId.Equals(id)).ToList();
            if (comments.Count > 0)
            {
                sqlContext.Comment.RemoveRange(comments);
            }

            var lessonApprovalHistories = sqlContext.LessonApprovalHistory.Where(i => i.LessonId.Equals(id)).ToList();
            if (lessonApprovalHistories.Count > 0)
            {
                sqlContext.LessonApprovalHistory.RemoveRange(lessonApprovalHistories);
            }

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa bài giảng: " + lessonExist.Name;

            sqlContext.Lesson.Remove(lessonExist);
            await sqlContext.SaveChangesAsync();


            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Danh sách câu hỏi
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<List<LessonQuestionModel>> SaerchQuestion(QuestionSearchConditionModel searchModel)
        {
            int index = 1;
            var data = (from a in sqlContext.Question.AsNoTracking()
                        join b in sqlContext.Topic.AsNoTracking() on a.TopicId equals b.Id
                        where !searchModel.ListId.Contains(a.Id) && a.ApprovalStatus == Constants.Course_Approval_Approved
                        orderby b.Name
                        select new LessonQuestionModel
                        {
                            Id = a.Id,
                            TopicId = a.TopicId,
                            TopicName = b.Name,
                            Type = a.Type,
                            Content = a.Content
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.TopicId))
            {
                data = data.Where(i => i.TopicId.Equals(searchModel.TopicId));
            }

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(i => i.TopicName.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (searchModel.Type.HasValue)
            {
                data = data.Where(i => i.Type == searchModel.Type);
            }

            var result = await data.ToListAsync();
            foreach (var item in result)
            {
                item.Index = index++;
            }

            return result;
        }

        /// <summary>
        /// Danh sách câu hỏi random
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<List<LessonQuestionModel>> GetListQuestionRandom(QuestionRandomModel model)
        {
            List<LessonQuestionModel> listQuestion = new List<LessonQuestionModel>();
            var data = await (from a in sqlContext.Question.AsNoTracking()
                              join b in sqlContext.Topic.AsNoTracking() on a.TopicId equals b.Id
                              where !model.ListId.Contains(a.Id) && model.ListTopic.Contains(a.TopicId) && a.ApprovalStatus == Constants.Course_Approval_Approved
                              orderby b.Name
                              select new LessonQuestionModel
                              {
                                  Id = a.Id,
                                  TopicId = a.TopicId,
                                  TopicName = b.Name,
                                  Type = a.Type
                              }).ToListAsync();

            if (data.Count > 0 && data.Count >= model.NumberQuestion)
            {
                List<int> listNumber = new List<int>();
                Random(0, data.Count, model.NumberQuestion, listNumber);
                foreach (var item in listNumber)
                {
                    listQuestion.Add(data[item]);
                }
            }
            else
            {
                throw NTSException.CreateInstance("Số lượng câu hỏi của chủ đề không đủ yêu cầu!");
            }

            return listQuestion;
        }

        private List<int> Random(int min, int max, int total, List<int> listNumber)
        {
            Random random = new Random();
            var rd = random.Next(min, max);
            if (listNumber.IndexOf(rd) == -1)
            {
                listNumber.Add(rd);
                if (listNumber.Count() < total)
                {
                    Random(min, max, total, listNumber);
                }
            }
            else
            {
                Random(min, max, total, listNumber);
            }

            return listNumber;
        }

        /// <summary>
        /// Yêu cầu duyệt bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RequestLessonAsync(string id, string userId, StatusModel model)
        {
            var lesson = sqlContext.Lesson.FirstOrDefault(i => i.Id.Equals(id));
            if (lesson == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = sqlContext.Database.BeginTransaction())
            {
                lesson.ApprovalStatus = Constants.Course_Approval_Request;
                lesson.RequestBy = userId;
                lesson.RequestDate = DateTime.Now;

                // Lưu lịch sử
                string action = $"Tài khoản {user.UserName} đã yêu cầu duyệt.";
                ApprovalHistorys(lesson, model.Content, action);

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
        public async Task ApprovalLessonAsync(string id, string userId, StatusModel model)
        {
            var lesson = sqlContext.Lesson.FirstOrDefault(i => i.Id.Equals(id));
            if (lesson == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }

            var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = sqlContext.Database.BeginTransaction())
            {
                lesson.ApprovalStatus = model.Status;
                lesson.ApprovalBy = userId;
                lesson.ApprovalDate = DateTime.Now;

                // Lưu lịch sử
                string action = string.Empty;
                if (model.Status == Constants.Course_Approval_Approved)
                {
                    action = $"Tài khoản {user.UserName} đã duyệt.";
                }
                else if (model.Status == Constants.Course_Approval_Creating)
                {
                    action = $"Tài khoản {user.UserName} không duyệt.";
                }
                else if (model.Status == Constants.Course_Approval_NotApproved)
                {
                    action = $"Tài khoản {user.UserName} hủy duyệt.";
                }
                else if (model.Status == Constants.Course_Approval_NotBrowse)
                {
                    action = $"Tài khoản {user.UserName} không duyệt.";
                }
                ApprovalHistorys(lesson, model.Content, action);

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

        private void ApprovalHistorys(Lesson lesson, string content, string action)
        {
            LessonApprovalHistory approvalHistory = new LessonApprovalHistory()
            {
                LessonId = lesson.Id,
                Action = action,
                Content = content,
                ApprovalStatus = lesson.ApprovalStatus,
                ProcessingDate = DateTime.Now
            };

            sqlContext.LessonApprovalHistory.Add(approvalHistory);
        }

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<LessonApprovalHistoryModel>> GetListLessonApprovalStatusAsync(string id)
        {
            var data = await (from a in sqlContext.LessonApprovalHistory.AsNoTracking()
                              where a.LessonId.Equals(id)
                              orderby a.ProcessingDate descending
                              select new LessonApprovalHistoryModel
                              {
                                  Id = a.Id,
                                  LessonId = a.LessonId,
                                  Action = a.Action,
                                  Content = a.Content,
                                  ApprovalStatus = a.ApprovalStatus,
                                  ProcessingDate = a.ProcessingDate
                              }).ToListAsync();

            return data;
        }
    }
}
