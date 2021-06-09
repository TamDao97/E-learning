using Elearning.Model.Entities;
using Elearning.Model.Models.Comment;
using Elearning.Model.Models.Lesson;
using Elearning.Model.Models.LessonFrame;
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
using NTS.Common.Resource;
using NTS.Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;

namespace Elearning.Services.Comments
{
    public class CommentService : ICommentService
    {
        private readonly ElearningContext sqlContext;
        private readonly IComboboxService comboboxService;
        private readonly IDetectionService _detection;


        public CommentService(ElearningContext sqlContext, IComboboxService comboboxService, IDetectionService _detection)
        {
            this.sqlContext = sqlContext;
            this.comboboxService = comboboxService;
            this._detection = _detection;
        }

        /// <summary>
        /// Tìm kiếm bình luận
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultCommentModel<CommentSearchResultModel>> SearchCommentAsync(CommentSearchConditionModel searchModel, string employeeId, string userId)
        {
            var listLesson = new List<string>();
            var listCourse = new List<string>();
            var user = sqlContext.User.Find(userId);
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }
            if (user.Type == Constants.User_UserType_Admin)
            {
                //listLesson = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                //              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                //              join c in sqlContext.Lesson.AsNoTracking() on b.LessonId equals c.Id
                //              where user.ManagerUnitId.Equals(c.ManagerUnitId)
                //              select b.LessonId).ToList();

                //listCourse = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                //              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                //              join c in sqlContext.Course.AsNoTracking() on a.CourseId equals c.Id
                //              where user.ManagerUnitId.Equals(c.ManagerUnitId)
                //              select b.CourseId).ToList();

                if (user.ManagerUnitLevel == 1)
                {
                    listLesson = (from a in sqlContext.LessonCourse.AsNoTracking()
                                  join b in sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                  select b.Id).ToList();

                    listCourse = (from a in sqlContext.LessonCourse.AsNoTracking()
                                  join b in sqlContext.Course.AsNoTracking() on a.CourseId equals b.Id
                                  select b.Id).ToList();
                }
                else
                {
                    listLesson = (from a in sqlContext.LessonCourse.AsNoTracking()
                                  join b in sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                  where b.ManagerUnitId.Equals(user.ManagerUnitId)
                                  select b.Id).ToList();

                    listCourse = (from a in sqlContext.LessonCourse.AsNoTracking()
                                  join b in sqlContext.Course.AsNoTracking() on a.CourseId equals b.Id
                                  where b.ManagerUnitId.Equals(user.ManagerUnitId)
                                  select b.Id).ToList();
                }
            }
            else if (user.Type == Constants.User_UserType_Expert)
            {
                listLesson = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                              select b.LessonId).ToList();

                listCourse = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                              select b.CourseId).ToList();
            }
            else
            {
                listLesson = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                              where a.EmployeeId.Equals(employeeId)
                              select b.LessonId).ToList();

                listCourse = (from a in sqlContext.EmployeeCourse.AsNoTracking()
                              join b in sqlContext.LessonCourse.AsNoTracking() on a.CourseId equals b.CourseId
                              where a.EmployeeId.Equals(employeeId)
                              select b.CourseId).ToList();
            }

            listLesson = listLesson.GroupBy(i => i).Select(i => i.Key).ToList();
            listCourse = listCourse.GroupBy(i => i).Select(i => i.Key).ToList();

            SearchBaseResultCommentModel<CommentSearchResultModel> searchResult = new SearchBaseResultCommentModel<CommentSearchResultModel>();
            var data = await (from a in sqlContext.Comment.AsNoTracking()
                              join b in sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id
                              join c in sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
                              join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                              join e in sqlContext.Lesson.AsNoTracking() on a.LessonId equals e.Id
                              join f in sqlContext.Category.AsNoTracking() on e.CategoryId equals f.Id
                              where a.Type == Constants.Comment_Type_Lesson && (a.ObjectType == Constants.User_UserType_Instructor || a.ObjectType == Constants.User_UserType_Admin) && listLesson.Contains(e.Id)
                              select new CommentSearchResultModel
                              {
                                  Id = a.Id,
                                  ParentCommentId = a.ParentCommentId,
                                  ParentCategoryId = f.ParentCategoryId,
                                  CourseId = a.CourseId,
                                  LessonId = a.LessonId,
                                  ObjectId = a.ObjectId,
                                  ObjectType = a.ObjectType,
                                  Content = a.Content,
                                  CommentDate = a.CommentDate,
                                  Status = a.Status,
                                  ImagePath = c.Avatar,
                                  Email = c.Email,
                                  AcountName = b.UserName,
                                  LessonName = e.Name,
                                  CategoryName = f.Name,
                              }).ToListAsync();

            data.AddRange((from a in sqlContext.Comment.AsNoTracking()
                           join c in sqlContext.Learner.AsNoTracking() on a.ObjectId equals c.Id
                           join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                           join e in sqlContext.Lesson.AsNoTracking() on a.LessonId equals e.Id
                           join f in sqlContext.Category.AsNoTracking() on e.CategoryId equals f.Id

                           where a.Type == Constants.Comment_Type_Lesson && a.ObjectType == Constants.User_UserType_Student && listLesson.Contains(e.Id)
                           select new CommentSearchResultModel
                           {
                               Id = a.Id,
                               ParentCategoryId = f.ParentCategoryId,
                               CategoryName = f.Name,
                               ParentCommentId = a.ParentCommentId,
                               CourseId = a.CourseId,
                               LessonId = a.LessonId,
                               ObjectId = a.ObjectId,
                               ObjectType = a.ObjectType,
                               Content = a.Content,
                               CommentDate = a.CommentDate,
                               Status = a.Status,
                               ImagePath = c.Avatar,
                               Email = c.Email,
                               AcountName = c.Name,
                               LessonName = e.Name
                           }).ToList());

            // Comment khóa học
            data.AddRange((from a in sqlContext.Comment.AsNoTracking()
                           join b in sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id
                           join c in sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
                           join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                           join f in sqlContext.Program.AsNoTracking() on d.ProgramId equals f.Id
                           where a.Type == Constants.Comment_Type_Course && (a.ObjectType == Constants.User_UserType_Instructor || a.ObjectType == Constants.User_UserType_Admin) && listCourse.Contains(d.Id)
                           select new CommentSearchResultModel
                           {
                               Id = a.Id,
                               ParentCommentId = a.ParentCommentId,
                               ParentCategoryId = f.Id,
                               CourseId = a.CourseId,
                               LessonId = a.LessonId,
                               ObjectId = a.ObjectId,
                               ObjectType = a.ObjectType,
                               Content = a.Content,
                               CommentDate = a.CommentDate,
                               Status = a.Status,
                               ImagePath = c.Avatar,
                               Email = c.Email,
                               AcountName = b.UserName,
                               LessonName = d.Name,
                               CategoryName = f.Name,
                           }).ToList());

            data.AddRange((from a in sqlContext.Comment.AsNoTracking()
                           join c in sqlContext.Learner.AsNoTracking() on a.ObjectId equals c.Id
                           join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                           join f in sqlContext.Program.AsNoTracking() on d.ProgramId equals f.Id

                           where a.Type == Constants.Comment_Type_Course && a.ObjectType == Constants.User_UserType_Student && listCourse.Contains(d.Id)
                           select new CommentSearchResultModel
                           {
                               Id = a.Id,
                               ParentCategoryId = f.Id,
                               CategoryName = f.Name,
                               ParentCommentId = a.ParentCommentId,
                               CourseId = a.CourseId,
                               LessonId = a.LessonId,
                               ObjectId = a.ObjectId,
                               ObjectType = a.ObjectType,
                               Content = a.Content,
                               CommentDate = a.CommentDate,
                               Status = a.Status,
                               ImagePath = c.Avatar,
                               Email = c.Email,
                               AcountName = c.Name,
                               LessonName = d.Name
                           }).ToList());

            searchResult.TotalItems = data.Count(a => a.ObjectType == 4 && a.ParentCommentId == null);
            searchResult.Pending = data.Where(i => i.Status == Constants.Comment_Status_Pending && i.ObjectType == 4 && i.ParentCommentId == null).Count();
            searchResult.Approved = data.Where(i => i.Status == Constants.Comment_Status_Approved && i.ObjectType == 4 && i.ParentCommentId == null).Count();
            searchResult.Delete = data.Where(i => i.Status == Constants.Comment_Status_Delete && i.ObjectType == 4 && i.ParentCommentId == null).Count();

            if (searchModel.Status.HasValue)
            {
                data = data.Where(i => i.Status == searchModel.Status).ToList();
            }
            if (!string.IsNullOrEmpty(searchModel.Content))
            {
                data = data.Where(i => i.Content.Trim().ToUpper().Contains(searchModel.Content.Trim().ToUpper())).ToList();
            }

            searchResult.TotalNew = data.Count();

            var list = data.Where(i => !i.ParentCommentId.HasValue).ToList();

            foreach (var item in list)
            {
                item.ListReply.AddRange(data.Where(i => i.ParentCommentId.HasValue && i.ParentCommentId.Value == item.Id).OrderBy(i => i.CommentDate));
                item.Total = data.Where(i => i.ParentCommentId.Equals(item.ParentCommentId)).Count();
            }

            searchResult.DataResults = list.OrderByDescending(a => a.CommentDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();
            //searchResult.DataResultsAll = list.OrderByDescending(a => a.CommentDate).ToList();

            var lists = await comboboxService.GetCategoryAsync();
            ComboboxParentModel comboboxParent;
            foreach (var item in searchResult.DataResults)
            {
                comboboxParent = new ComboboxParentModel();
                if (!string.IsNullOrEmpty(item.ParentCategoryId))
                {
                    item.CategoryName = GetCategoryName(item.CategoryName, item.ParentCategoryId, lists);
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
        /// Thêm mới bình luận
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateCommentAsync(HttpRequest request, CommentCreateModel model, string userId)
        {
            var parentComment = sqlContext.Comment.FirstOrDefault(i => i.Id == model.ParentCommentId);
            if (parentComment != null)
            {
                parentComment.Status = Constants.Comment_Status_Approved;
                var listParent = GetChildComment(sqlContext.Comment.ToList(), parentComment.Id);
                foreach (var item in listParent)
                {
                    item.Status = Constants.Comment_Status_Approved; ;
                }
            }

            var user = sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            Comment comment = new Comment()
            {
                ParentCommentId = model.ParentCommentId,
                CourseId = model.CourseId,
                LessonId = model.LessonId,
                ObjectId = user.Id,
                ObjectType = user.Type,
                Type = parentComment.Type,
                Content = model.Content,
                CommentDate = DateTime.Now,
                Status = model.Status
            };
            sqlContext.Comment.Add(comment);
            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, user.Id);
            userHistory.Content = "Thêm mới bình luận: " + model.Content;
            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Cập nhật bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task UpdateCommentAsync(HttpRequest request, long id, CommentCreateModel model, NtsUserTokenModel user)
        {
            var comment = await sqlContext.Comment.FirstOrDefaultAsync(i => i.Id == id);
            string NameOld = comment.Content;
            if (comment == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Comment);
            }
            comment.ParentCommentId = model.ParentCommentId;
            comment.CourseId = model.CourseId;
            comment.LessonId = model.LessonId;
            comment.ObjectId = user.UserId;
            comment.ObjectType = user.Type;
            comment.Content = model.Content;
            comment.CommentDate = DateTime.Now;
            comment.Status = model.Status;

            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, user.UserId);

            if (NameOld.ToLower() == model.Content.ToLower())
            {
                userHistory.Content = "Cập nhật bình luận có nội dung: " + NameOld;
            }
            else
            {
                userHistory.Content = "Cập nhật bình luận có nội dung đầu là: " + NameOld + " thành " + model.Content;
            }
            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Lấy thông tin bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<CommentCreateModel> GetCommentByIdAsync(long id, string userId)
        {
            var resultInfo = await (from a in sqlContext.Comment.AsNoTracking()
                                    where a.Id == id
                                    select new CommentCreateModel()
                                    {
                                        Id = a.Id,
                                        ParentCommentId = a.ParentCommentId,
                                        CourseId = a.CourseId,
                                        LessonId = a.LessonId,
                                        ObjectId = a.ObjectId,
                                        ObjectType = a.ObjectType,
                                        Type = a.Type,
                                        Content = a.Content,
                                        CommentDate = a.CommentDate,
                                        Status = a.Status
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Comment);
            }

            return resultInfo;
        }

        /// <summary>
        /// Xóa bình luận
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task DeleteCommentByIdAsync(HttpRequest request, long id, string userId)
        {
            var commentExist = await sqlContext.Comment.FirstOrDefaultAsync(i => i.Id == id);
            if (commentExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Comment);
            }

            var listData = sqlContext.Comment.ToList();
            var listParent = GetChildComment(listData, commentExist.Id);
            if (listParent.Count > 0)
            {
                sqlContext.Comment.RemoveRange(listParent);
            }

            sqlContext.Comment.Remove(commentExist);
            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Xóa bình luận: " + commentExist.Content;
            LogService.Event(userHistory, _detection);
        }

        public List<Comment> GetChildComment(List<Comment> comments, long parentId)
        {
            List<Comment> list = new List<Comment>();

            var listParent = comments.Where(i => i.ParentCommentId == parentId).ToList();
            list.AddRange(listParent);
            foreach (var item in listParent)
            {
                list.AddRange(GetChildComment(comments, item.Id));
            }

            return list;
        }

        /// <summary>
        /// Duyệt bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        public async Task ApprovedComment(HttpRequest request, List<long> listComment, string userId)
        {
            var list = await sqlContext.Comment.ToListAsync();

            Comment comment;
            List<Comment> listComments;
            foreach (var item in listComment)
            {
                comment = new Comment();
                listComments = new List<Comment>();
                comment = list.FirstOrDefault(i => i.Id == item);
                if (comment != null)
                {
                    comment.Status = Constants.Comment_Status_Approved;
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Hiển thị bình luận: " + comment.Content;
                    LogService.Event(userHistory, _detection);
                }

                listComments = list.Where(i => i.ParentCommentId == item).ToList();
                foreach (var items in listComments)
                {
                    items.Status = Constants.Comment_Status_Approved;
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Hiển thị bình luận: " + items.Content;
                    LogService.Event(userHistory, _detection);
                }
            }

            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Ẩn bình luận
        /// </summary>
        /// <param name="listComment"></param>
        /// <returns></returns>
        public async Task UnApprovedComment(HttpRequest request, List<long> listComment, string userId)
        {
            var list = await sqlContext.Comment.ToListAsync();

            Comment comment;
            List<Comment> listComments;
            foreach (var item in listComment)
            {
                comment = new Comment();
                listComments = new List<Comment>();
                comment = list.FirstOrDefault(i => i.Id == item);
                if (comment != null)
                {
                    comment.Status = Constants.Comment_Status_Delete;
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Ẩn bình luận: " + comment.Content;
                    LogService.Event(userHistory, _detection);
                }

                listComments = list.Where(i => i.ParentCommentId == item).ToList();
                foreach (var items in listComments)
                {
                    items.Status = Constants.Comment_Status_Delete;
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Ẩn bình luận: " + items.Content;
                    LogService.Event(userHistory, _detection);
                }
            }

            await sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Lấy số bình luận mới
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<string> GetCommentTotalNewAsync(string employeeId)
        {
            string news = string.Empty;
            //var data = (from a in sqlContext.Comment.AsNoTracking()
            //            join b in sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id
            //            join c in sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
            //            join d in sqlContext.LessonCourse.AsNoTracking() on a.LessonCourseId equals d.Id
            //            join e in sqlContext.Lesson.AsNoTracking() on d.LessonId equals e.Id
            //            join f in sqlContext.EmployeeCourse on d.CourseId equals f.CourseId
            //            where f.EmployeeId.Equals(employeeId)
            //            select new CommentSearchResultModel
            //            {
            //                Id = a.Id,
            //            }).AsQueryable();


            //var totalNew = await data.CountAsync();
            //news = /*"<span style=\"font - size: 10px;\">"*/ totalNew.ToString()  /*+"</span>"*/;
            return news;
        }

        /// <summary>
        /// Tìm kiếm bình luận
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<CommentSearchResultFontEndModel>> SearchCommentFontEndAsync(CommentSearchConditionModel searchModel, string userId)
        {
            SearchBaseResultModel<CommentSearchResultFontEndModel> searchResult = new SearchBaseResultModel<CommentSearchResultFontEndModel>();
            var data = await (from a in sqlContext.Comment.AsNoTracking()
                              join b in sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id
                              join c in sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
                              join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                              join e in sqlContext.Lesson.AsNoTracking() on a.LessonId equals e.Id
                              join f in sqlContext.LessonFrame.AsNoTracking() on a.LessonFrameId equals f.Id
                              where a.Type == Constants.Comment_Type_Lesson && (a.ObjectType == Constants.User_UserType_Instructor || a.ObjectType == Constants.User_UserType_Admin) && e.Id.Equals(searchModel.LessonId) && f.Id.Equals(searchModel.LessonFrameId)
                              select new CommentSearchResultFontEndModel
                              {
                                  Id = a.Id,
                                  ParentCommentId = a.ParentCommentId,
                                  CourseId = a.CourseId,
                                  LessonId = a.LessonId,
                                  ObjectId = a.ObjectId,
                                  ObjectType = a.ObjectType,
                                  Content = a.Content,
                                  CommentDate = a.CommentDate,
                                  Status = a.Status,
                                  ImagePath = c.Avatar,
                                  Email = c.Email,
                                  AcountName = b.UserName,
                                  LessonName = e.Name
                              }).ToListAsync();

            data.AddRange((from a in sqlContext.Comment.AsNoTracking()
                           join c in sqlContext.Learner.AsNoTracking() on a.ObjectId equals c.Id
                           join d in sqlContext.Course.AsNoTracking() on a.CourseId equals d.Id
                           join e in sqlContext.Lesson.AsNoTracking() on a.LessonId equals e.Id
                           join f in sqlContext.LessonFrame.AsNoTracking() on a.LessonFrameId equals f.Id
                           where a.Type == Constants.Comment_Type_Lesson && a.ObjectType == Constants.User_UserType_Student && e.Id.Equals(searchModel.LessonId) && e.Id.Equals(searchModel.LessonId) && f.Id.Equals(searchModel.LessonFrameId)
                           select new CommentSearchResultFontEndModel
                           {
                               Id = a.Id,
                               ParentCommentId = a.ParentCommentId,
                               CourseId = a.CourseId,
                               LessonId = a.LessonId,
                               ObjectId = a.ObjectId,
                               ObjectType = a.ObjectType,
                               Content = a.Content,
                               CommentDate = a.CommentDate,
                               Status = a.Status,
                               ImagePath = c.Avatar,
                               Email = c.Email,
                               AcountName = c.Name,
                               LessonName = e.Name
                           }).ToList());

            data = data.Where(i => i.Status == Constants.Comment_Status_Approved || i.ObjectId.Equals(searchModel.UserId)).ToList();

            var list = data.Where(i => !i.ParentCommentId.HasValue).ToList();
            foreach (var item in list)
            {
                item.ListReply.AddRange(data.Where(i => i.ParentCommentId.HasValue && i.ParentCommentId.Value == item.Id).OrderBy(i => i.CommentDate));
                //item.Total = data.Where(i => i.ParentCommentId.Equals(item.ParentCommentId)).Count();
            }

            searchResult.TotalItems = list.Count();
            searchResult.DataResults = list.OrderBy(a => a.CommentDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            return searchResult;
        }

        /// <summary>
        /// Tìm kiếm bình luận khóa học
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<CommentSearchResultFontEndModel>> SearchCommentFontEndCourseAsync(CommentSearchConditionModel searchModel, string userId)
        {
            SearchBaseResultModel<CommentSearchResultFontEndModel> searchResult = new SearchBaseResultModel<CommentSearchResultFontEndModel>();
            var data = await (from a in sqlContext.Comment.AsNoTracking()
                              join b in sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id
                              join c in sqlContext.Employee.AsNoTracking() on b.ObjectId equals c.Id
                              where a.Type == Constants.Comment_Type_Course && (a.ObjectType == Constants.User_UserType_Instructor || a.ObjectType == Constants.User_UserType_Admin) && a.CourseId.Equals(searchModel.CourseId)
                              select new CommentSearchResultFontEndModel
                              {
                                  Id = a.Id,
                                  ParentCommentId = a.ParentCommentId,
                                  CourseId = a.CourseId,
                                  LessonId = a.LessonId,
                                  ObjectId = a.ObjectId,
                                  ObjectType = a.ObjectType,
                                  Content = a.Content,
                                  CommentDate = a.CommentDate,
                                  Status = a.Status,
                                  ImagePath = c.Avatar,
                                  Email = c.Email,
                                  AcountName = b.UserName,
                                  Provider = null,
                              }).ToListAsync();

            data.AddRange((from a in sqlContext.Comment.AsNoTracking()
                           join c in sqlContext.Learner.AsNoTracking() on a.ObjectId equals c.Id
                           where a.Type == Constants.Comment_Type_Course && a.ObjectType == Constants.User_UserType_Student && a.CourseId.Equals(searchModel.CourseId)
                           select new CommentSearchResultFontEndModel
                           {
                               Id = a.Id,
                               ParentCommentId = a.ParentCommentId,
                               CourseId = a.CourseId,
                               LessonId = a.LessonId,
                               ObjectId = a.ObjectId,
                               ObjectType = a.ObjectType,
                               Content = a.Content,
                               CommentDate = a.CommentDate,
                               Status = a.Status,
                               ImagePath = c.Avatar,
                               Email = c.Email,
                               AcountName = c.Name,
                               Provider = c.Provider,
                           }).ToList());

            data = data.Where(i => i.Status == Constants.Comment_Status_Approved || i.ObjectId.Equals(searchModel.UserId)).ToList();

            var list = data.Where(i => !i.ParentCommentId.HasValue).ToList();
            foreach (var item in list)
            {
                item.ListReply.AddRange(data.Where(i => i.ParentCommentId.HasValue && i.ParentCommentId.Value == item.Id).OrderBy(i => i.CommentDate));
                //item.Total = data.Where(i => i.ParentCommentId.Equals(item.ParentCommentId)).Count();
            }

            searchResult.TotalItems = list.Count();
            searchResult.DataResults = list.OrderBy(a => a.CommentDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToList();

            return searchResult;
        }

        /// <summary>
        /// Lấy thông tin khóa học và danh sách bài giảng
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<object> GetLessonCourseAsync(HttpRequest request, string courseId, Model.Models.Course.LessonCourse model)
        {
            var result = await (from a in sqlContext.Course.AsNoTracking()
                                where a.Id.Equals(courseId)
                                select new CourseFontendModel
                                {
                                    Id = a.Id,
                                    Name = a.Name
                                }).FirstOrDefaultAsync();

            string lessionId = null;

            if (result != null)
            {
                result.ListLessonCourse = (from a in sqlContext.LessonCourse.AsNoTracking()
                                           join b in sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                           where a.CourseId.Equals(result.Id)
                                           orderby a.DisplayIndex
                                           select new LessonCoursesModel
                                           {
                                               Id = a.Id,
                                               LessonId = a.LessonId,
                                               LessonName = b.Name,
                                               CourseId = a.CourseId,
                                               Index = a.DisplayIndex,
                                               ImagePath = b.ImagePath,
                                               Type = b.Type
                                           }).ToList();
                LessonHistory lessonHistory;
                foreach (var item in result.ListLessonCourse)
                {
                    lessonHistory = new LessonHistory();
                    lessonHistory = sqlContext.LessonHistory.FirstOrDefault(i => i.LessonId.Equals(item.LessonId) && i.LearnerId.Equals(model.UserId) && i.CourseId.Equals(item.CourseId));
                    if (lessonHistory != null)
                    {
                        item.Status = true;
                    }
                }

                lessionId = result.ListLessonCourse.Where(c => c.Type == 3).FirstOrDefault()?.LessonId;
            }

            var testInfo = sqlContext.Test.Where(a => a.LearnerId.Equals(model.UserId) && a.LessonId.Equals(lessionId) && a.CourseId.Equals(courseId)).FirstOrDefault();

            DateTime? StartDate = null;
            DateTime? FinishDate = null;
            if (testInfo != null)
            {
                StartDate = testInfo.StartDate;
                FinishDate = testInfo.FinishDate;
            }

            return new
            {
                result,
                StartDate,
                FinishDate
            };
        }

        /// <summary>
        /// Lấy thông tin bài giảng theo id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LessonCreateModel> GetLessonByIdAsync(string id, CommentCreateModel model)
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
                                        ExamTime = a.ExamTime
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }
            if (!string.IsNullOrEmpty(model.ObjectId))
            {
                var checkLearnerCourse = sqlContext.LearnerCourse.FirstOrDefault(s => s.LearnerId == model.ObjectId && s.CourseId == model.CourseId);
                if (checkLearnerCourse != null)
                {
                    var check = sqlContext.LessonHistory.FirstOrDefault(i => i.LessonId.Equals(model.LessonId) && i.LearnerId.Equals(model.ObjectId) && i.CourseId.Equals(model.CourseId));
                    if (check == null)
                    {
                        LessonHistory lessonHistory = new LessonHistory()
                        {
                            //Id = Guid.NewGuid().ToString(),
                            LessonId = model.LessonId,
                            LearnerId = model.ObjectId,
                            CourseId = model.CourseId,
                            StartDate = DateTime.Now
                        };
                        sqlContext.LessonHistory.Add(lessonHistory);
                        sqlContext.SaveChanges();
                    }
                }
            }

            return resultInfo;
        }

        /// <summary>
        /// Thêm mới bình luận font-end
        /// </summary>
        /// <param name="model"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateCommentFontEndAsync(HttpRequest request, CommentCreateModel model)
        {
            Comment comment = new Comment()
            {
                ParentCommentId = model.ParentCommentId,
                CourseId = model.CourseId,
                LessonId = model.LessonId,
                LessonFrameId = model.LessonFrameId,
                ObjectId = model.ObjectId,
                ObjectType = Constants.User_UserType_Student,
                Type = model.Type,
                Content = model.Content,
                CommentDate = DateTime.Now,
                Status = model.Status
            };
            sqlContext.Comment.Add(comment);
            await sqlContext.SaveChangesAsync();

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, model.ObjectId);

            if (comment.ObjectType == Constants.User_UserType_Student)
            {
                var learnerName = sqlContext.Learner.FirstOrDefault(a => a.Id.Equals(model.ObjectId));
                if (learnerName != null)
                {
                    userHistory.Content = learnerName.Name + " đã bình luận: " + model.Content;
                    userHistory.Type = 1;
                }
            }
            else
            {
                var learnerName = sqlContext.User.FirstOrDefault(a => a.Id.Equals(model.ObjectId));
                if (learnerName != null)
                {
                    userHistory.Content = "Tài khoản: " + learnerName.UserName + " đã bình luận: " + model.Content;
                    userHistory.Type = 0;
                }
            }

            LogService.Event(userHistory, _detection);
        }

        /// <summary>
        /// Lấy danh mục con bài giảng
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LessonFrameModel> GetLessonFrameByIdAsync(string id, CommentCreateModel model)
        {
            var resultInfo = await (from a in sqlContext.LessonFrame.AsNoTracking()
                                    where a.Id.Equals(id)
                                    select new LessonFrameModel()
                                    {
                                        Id = a.Id,
                                        LessonId = a.LessonId,
                                        Name = a.Name,
                                        Content = a.Content,
                                        Type = a.Type,
                                        EstimatedTime = a.EstimatedTime,
                                        TestTime = a.TestTime,
                                        TotalRequestCorrect = a.TotalRequestCorrect,
                                        TotalQuestion = a.TotalQuestion,
                                        DisplayIndex = a.DisplayIndex
                                    }).FirstOrDefaultAsync();
            if (resultInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }
            if (model.ObjectId != null)
            {
                //var checkLearnerCourse = sqlContext.LearnerCourse.FirstOrDefault(s => s.LearnerId == model.ObjectId && s.CourseId == model.CourseId);
                //if (checkLearnerCourse != null)
                //{
                var check = sqlContext.LessonFrameHistory.FirstOrDefault(i => i.LessonId.Equals(model.LessonId) && i.LearnerId.Equals(model.ObjectId) && i.CourseId.Equals(model.CourseId) && i.LessonFrameId.Equals(resultInfo.Id));
                if (check == null)
                {
                    LessonFrameHistory lessonFrameHistory = new LessonFrameHistory()
                    {
                        //Id = Guid.NewGuid().ToString(),
                        LessonFrameId = resultInfo.Id,
                        LessonId = model.LessonId,
                        LearnerId = model.ObjectId,
                        CourseId = model.CourseId,
                        StartDate = DateTime.Now,
                        Pass = true,
                    };
                    sqlContext.LessonFrameHistory.Add(lessonFrameHistory);
                    sqlContext.SaveChanges();
                }
                //}
            }

            return resultInfo;
        }
    }
}
