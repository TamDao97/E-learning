using Elearning.Model.Entities;
using Elearning.Model.Models.Answer;
using Elearning.Model.Models.ApprovalHistory;
using Elearning.Model.Models.Base;
using Elearning.Model.Models.Course;
using Elearning.Model.Models.Fontend.Exam;
using Elearning.Model.Models.Question;
using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;
using Elearning.Model.Models.UserHistory;
using Elearning.Models.Base;
using Elearning.Models.Combobox;
using Elearning.Models.Entities;
using Elearning.Services.Log;
using Elearning.Services.UserDevice;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using Syncfusion.DocIO;
using Syncfusion.DocIO.DLS;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wangkanai.Detection.Services;
using NTS.Common.Helpers;
using Elearning.Model.Models.LessonFrame;
using Elearning.Model.Models.Lesson;

namespace Elearning.Services.Course
{
    public class CourseService : ICourseService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IDetectionService _detection;


        public CourseService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this._sqlContext = sqlContext;
            this._detection = _detection;

        }

        /// <summary>
        /// Lấy danh sách mentor theo khóa học
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<MentorResultModel>> SearchMentor(string id)
        {
            var data = (from a in _sqlContext.EmployeeCourse.AsNoTracking()
                        where a.CourseId.Equals(id)
                        join b in _sqlContext.Employee.AsNoTracking() on a.EmployeeId equals b.Id
                        join c in _sqlContext.User.AsNoTracking() on b.Id equals c.ObjectId
                        join d in _sqlContext.ManagerUnit.AsNoTracking() on c.ManagerUnitId equals d.Id
                        select new MentorResultModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            PhoneNumber = b.PhoneNumber,
                            Email = b.Email,
                            Address = b.Address,
                            ManageUnit = d.Name,
                            Logo = d.Logo,
                            Avatar = b.Avatar
                        }).AsQueryable();

            SearchBaseResultModel<MentorResultModel> searchResult = new SearchBaseResultModel<MentorResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.ToListAsync();

            return searchResult;
        }

        public async Task DeleteCourseByIdAsync(HttpRequest request, string id, string userId)
        {
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                var maxIndex = _sqlContext.Course.AsNoTracking().Select(a => a.DisplayIndex).Max();
                var courseExist = await _sqlContext.Course.FindAsync(id);
                var courseExistName = _sqlContext.Course.FindAsync(id).Result.Name;
                if (courseExist == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
                }

                if (courseExist.DisplayIndex <= maxIndex)
                {
                    int modelIndex = courseExist.DisplayIndex;
                    var listCourse = _sqlContext.Course.AsNoTracking().Where(b => b.DisplayIndex >= modelIndex).ToList();
                    if (listCourse.Count() > 0 && listCourse != null)
                    {
                        foreach (var item in listCourse)
                        {
                            if (!item.Id.Equals(courseExist.Id))
                            {
                                var updateUnit = _sqlContext.Course.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();

                                updateUnit.DisplayIndex--;
                            }

                        }
                    }
                }
                var checkLearner = _sqlContext.LearnerCourse.Where(s => s.CourseId == id).Count();
                if (checkLearner > 0)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0004, TextResourceKey.Course);
                }
                var checkEmployee = _sqlContext.EmployeeCourse.Where(s => s.CourseId == id);
                _sqlContext.EmployeeCourse.RemoveRange(checkEmployee);

                var checkLesson = _sqlContext.LessonCourse.Where(s => s.CourseId == id);
                _sqlContext.LessonCourse.RemoveRange(checkLesson);

                var checkComment = _sqlContext.Comment.Where(s => s.CourseId == id);
                _sqlContext.Comment.RemoveRange(checkComment);

                var checkApprovalHistory = _sqlContext.ApprovalHistory.Where(i => i.CourseId.Equals(id));
                _sqlContext.ApprovalHistory.RemoveRange(checkApprovalHistory);

                _sqlContext.Course.Remove(courseExist);
                try
                {
                    _sqlContext.SaveChanges();
                    trans.Commit();
                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Xóa khóa học: " + courseExistName;
                    LogService.Event(userHistory, _detection);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }

        public async Task<SearchBaseStatusModel<CourseResultModel>> SearchCourseAsync(CourseSearchModel searchModel, int level, string managerUnitId)
        {
            var data = (from a in _sqlContext.Course.AsNoTracking()
                        join b in _sqlContext.Program.AsNoTracking() on a.ProgramId equals (b.Id)
                        join c in _sqlContext.User.AsNoTracking() on a.CreateBy equals c.Id
                        join d in _sqlContext.ManagerUnit.AsNoTracking() on c.ManagerUnitId equals d.Id
                        join e in _sqlContext.Employee.AsNoTracking() on c.ObjectId equals e.Id
                        select new CourseResultModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            ProgramId = a.ProgramId,
                            ProgramName = b.Name,
                            StartDate = a.StartDate,
                            FinishDate = a.FinishDate,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            Status = a.Status,
                            DisplayIndex = a.DisplayIndex,
                            ApprovalStatus = a.ApprovalStatus,
                            ManageUnitId = c.ManagerUnitId,
                            CreateDate = a.CreateDate,
                            CreateBy = e.Name,
                            ManageUnit = d.Name,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(searchModel.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(searchModel.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(searchModel.ProgramId))
            {
                data = data.Where(r => r.ProgramId == searchModel.ProgramId);
            }
            if (!string.IsNullOrEmpty(searchModel.ManageUnitId))
            {
                data = data.Where(r => r.ManageUnitId == searchModel.ManageUnitId);
            }

            if (level != 1)
            {
                data = data.Where(i => i.ManageUnitId.Equals(managerUnitId));
            }

            SearchBaseStatusModel<CourseResultModel> searchResult = new SearchBaseStatusModel<CourseResultModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Count();
            searchResult.TotalCreating = data.Where(i => i.ApprovalStatus == Constants.Course_Approval_Creating).Count();
            searchResult.TotalRequest = data.Where(i => i.ApprovalStatus == Constants.Course_Approval_Request).Count();
            searchResult.TotalApproval = data.Where(i => i.ApprovalStatus == Constants.Course_Approval_Approved).Count();
            searchResult.TotalNotApproval = data.Where(i => i.ApprovalStatus == Constants.Course_Approval_NotApproved).Count();
            searchResult.TotalNotBrowse = data.Where(i => i.ApprovalStatus == Constants.Course_Approval_NotBrowse).Count();

            if (searchModel.ApprovalStatus.HasValue)
            {
                data = data.Where(i => i.ApprovalStatus == searchModel.ApprovalStatus);
            }

            searchResult.DataResults = await data.OrderByDescending(s => s.CreateDate).Skip((searchModel.PageNumber - 1) * searchModel.PageSize).Take(searchModel.PageSize).ToListAsync();
            return searchResult;
        }

        public async Task UpdateStatusCourseAsync(HttpRequest request, string id, string userId)
        {
            var courseExist = _sqlContext.Course.Find(id);
            var courseExistName = _sqlContext.Course.Find(id).Name;
            if (courseExist == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }
            if (courseExist.Status == true)
            {
                courseExist.Status = false;
            }
            else
            {
                courseExist.Status = true;
            }
            _sqlContext.SaveChanges();

            var statusName = this.getNameStatus(courseExist.Status);

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
            userHistory.Content = "Cập nhật trạng thái khóa học: " + courseExistName + " thành " + statusName;
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

        public async Task CreateCourseAsync(HttpRequest request, CourseCreateModel model, string userId, string manageUnit)
        {
            var indexs = _sqlContext.Course.ToList();
            var maxIndex = 2;
            if (indexs.Count > 0)
            {
                maxIndex = indexs.Select(a => a.DisplayIndex).Max();
            }

            if (model.DisplayIndex <= maxIndex)
            {
                int modelIndex = model.DisplayIndex;
                var listOrder = _sqlContext.Course.AsNoTracking().Where(b => b.DisplayIndex >= modelIndex).ToList();
                if (listOrder.Count() > 0 && listOrder != null)
                {
                    foreach (var item in listOrder)
                    {
                        var updateOrder = _sqlContext.Course.Where(r => r.Id.Equals(item.Id)).FirstOrDefault();
                        updateOrder.DisplayIndex++;
                    }
                }
            }
            var courseExist = _sqlContext.Course.AsNoTracking().FirstOrDefault(r => r.Name.ToUpper().Equals(model.Name.ToUpper()) && r.ProgramId.ToUpper().Equals(model.ProgramId.ToUpper()));
            if (courseExist != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Course);
            }
            if (model.StartDate > model.FinishDate)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0046, TextResourceKey.Course);
            }
            if (model.LessonCourses.Count == 0)
            {
                throw NTSException.CreateInstance("Khóa đào tạo chưa được xây dựng nội dung.");
            }
            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }
            var nameCourseExist = _sqlContext.Course.AsNoTracking().FirstOrDefault(r => r.Name.ToUpper().Equals(model.Name.ToUpper()));
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                Elearning.Model.Entities.Course course = new Elearning.Model.Entities.Course()
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = model.Name,
                    Description = model.Description,
                    Content = model.Content,
                    ProgramId = model.ProgramId,
                    FinishDate = model.FinishDate,
                    StartDate = model.StartDate,
                    DisplayIndex = model.DisplayIndex,
                    ImagePath = model.ImagePath,
                    Slug = nameCourseExist == null ? SlugHelper.ConverNameToSlug(model.Name) : SlugHelper.ConverNameToSlug(model.Name) + DateTime.Now.ToString("yyyyMMddHHmmss"),
                    Status = model.Status,
                    CreateBy = userId,
                    CreateDate = DateTime.Now,
                    UpdateBy = userId,
                    UpdateDate = DateTime.Now,
                    CertificateTemplateId = model.CertificateTemplateId,
                    ApprovalStatus = Constants.Course_Approval_Creating,
                    ManagerUnitId = manageUnit,
                };

                AddLearer(course.Id, model.LearnerCourses);
                AddEmployee(course.Id, model.EmployeeCourses);
                AddLesson(course.Id, model.LessonCourses, false);

                _sqlContext.Course.Add(course);

                // Lưu lịch sử
                string action = $"Tài khoản {user.UserName} đang tạo khóa học: {course.Name}";
                ApprovalHistorys(course, "", action);

                try
                {
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();

                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    userHistory.Content = "Thêm mới khóa học: " + model.Name;
                    LogService.Event(userHistory, _detection);
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        private void AddLesson(string id, List<LessonCourseModel> lessonCourses, bool isDelete)
        {
            var Check1Test = from a in lessonCourses
                             join b in _sqlContext.Lesson.AsNoTracking() on a.LessonId equals (b.Id)
                             where b.Type == Constants.Lesson_Type_Exam
                             select lessonCourses;
            if (Check1Test.ToList().Count > 1)
            {
                throw NTSException.CreateInstance("Có 2 bài thi cuối khóa trong khóa học.");
            }
            if ((lessonCourses.Count != 0 && lessonCourses != null))
            {
                var listLessonCourse = new List<Models.Entities.LessonCourse>();
                var listIdOld = _sqlContext.LessonCourse.Where(s => s.CourseId == id).Select(s => s.Id).ToList();
                var listIdnew = lessonCourses.Select(s => s.Id).ToList();
                listIdOld = listIdOld.Except(listIdnew).ToList();
                if (listIdOld.Count > 0 && listIdOld != null)
                {
                    foreach (var item in listIdOld)
                    {
                        var data = _sqlContext.LessonCourse.Where(s => s.Id == item).FirstOrDefault();
                        var lessonHistory = _sqlContext.LessonHistory.Where(s => s.CourseId == data.CourseId && s.LessonId == data.LessonId).ToList();
                        var comments = _sqlContext.Comment.Where(s => s.CourseId == data.CourseId && s.LessonId == data.LessonId).ToList();
                        var tests = _sqlContext.Test.Where(s => s.CourseId == data.CourseId && s.LessonId == data.LessonId).ToList();
                        foreach (var i in tests)
                        {
                            var answerLearner = _sqlContext.AnswerLearner.Where(s => s.TestId == i.Id).ToList();
                            _sqlContext.AnswerLearner.RemoveRange(answerLearner);
                        }
                        _sqlContext.Test.RemoveRange(tests);
                        _sqlContext.Comment.RemoveRange(comments);
                        _sqlContext.LessonHistory.RemoveRange(lessonHistory);
                        _sqlContext.LessonCourse.Remove(data);

                    }
                }

                Models.Entities.LessonCourse lessonCourse;
                foreach (var item in lessonCourses)
                {
                    if (string.IsNullOrEmpty(item.Id))
                    {
                        lessonCourse = new Models.Entities.LessonCourse
                        {
                            Id = Guid.NewGuid().ToString(),
                            CourseId = id,
                            DisplayIndex = item.DisplayIndex,
                            LessonId = item.LessonId,
                        };
                        listLessonCourse.Add(lessonCourse);
                    }
                    else
                    {
                        var oldLessonCourse = _sqlContext.LessonCourse.Find(item.Id);
                        if (oldLessonCourse == null)
                        {
                            throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.LessonCourse);
                        }

                        oldLessonCourse.DisplayIndex = item.DisplayIndex;
                    }
                }
                _sqlContext.LessonCourse.AddRange(listLessonCourse);
            }
            if ((lessonCourses.Count == 0 || lessonCourses == null) && isDelete == true)
            {
                var listOld = _sqlContext.LessonCourse.Where(s => s.CourseId == id).ToList();
                foreach (var item in listOld)
                {
                    var lessonHistory = _sqlContext.LessonHistory.Where(s => s.CourseId == item.CourseId && s.LessonId == item.LessonId).ToList();
                    _sqlContext.LessonHistory.RemoveRange(lessonHistory);
                    var comments = _sqlContext.Comment.Where(s => s.CourseId == item.CourseId && s.LessonId == item.LessonId).ToList();
                    var tests = _sqlContext.Test.Where(s => s.CourseId == item.CourseId && s.LessonId == item.LessonId).ToList();
                    _sqlContext.Test.RemoveRange(tests);
                    _sqlContext.Comment.RemoveRange(comments);
                }
                _sqlContext.LessonCourse.RemoveRange(listOld);
                throw NTSException.CreateInstance("Khóa đào tạo chưa được xây dựng nội dung.");
            }
        }

        private void AddEmployee(string id, List<string> employeeCourses)
        {

            var employee = _sqlContext.EmployeeCourse.Where(a => a.CourseId.Equals(id)).ToList();
            if (employee.Count > 0)
            {
                _sqlContext.EmployeeCourse.RemoveRange(employee);
            }

            if (employeeCourses.Count > 0)
            {
                foreach (var item in employeeCourses)
                {
                    Elearning.Model.Entities.EmployeeCourse employeeCourse = new Elearning.Model.Entities.EmployeeCourse()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CourseId = id,
                        EmployeeId = item,
                    };

                    _sqlContext.EmployeeCourse.Add(employeeCourse);
                }
            }
        }

        private void AddLearer(string id, List<string> learnerCourses)
        {
            var listLearnerCourse = new List<LearnerCourse>();
            var listIdOld = _sqlContext.LearnerCourse.Where(s => s.CourseId == id).Select(s => s.LearnerId).ToList();
            var listIdnew = learnerCourses.ToList();
            var listNew = listIdnew.Except(listIdOld).ToList();
            var listOld = listIdOld.Except(listIdnew).ToList();
            foreach (var item in listOld)
            {
                var learnerCourse = _sqlContext.LearnerCourse.Where(s => s.CourseId == id && s.LearnerId == item).ToList();
                _sqlContext.LearnerCourse.RemoveRange(learnerCourse);
                var lessonHistory = _sqlContext.LessonHistory.Where(s => s.CourseId == id && s.LearnerId == item).ToList();
                _sqlContext.LessonHistory.RemoveRange(lessonHistory);
                var test = _sqlContext.Test.Where(s => s.CourseId == id && s.LearnerId == item).ToList();
                _sqlContext.Test.RemoveRange(test);
            }
            foreach (var item in listNew)
            {
                var learnerCourse = new LearnerCourse
                {
                    Id = Guid.NewGuid().ToString(),
                    CertificateDatePrint = null,
                    RegistrationDate = DateTime.Now,
                    CourseId = id,
                    LearnerId = item,
                };
                listLearnerCourse.Add(learnerCourse);
            }
            _sqlContext.LearnerCourse.AddRange(listLearnerCourse);
        }

        public async Task UpdateCourseAsync(HttpRequest request, string id, CourseCreateModel model, string userId)
        {
            using (var trans = _sqlContext.Database.BeginTransaction())
            {
                var courseExist = _sqlContext.Course.AsNoTracking().FirstOrDefault(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower()) && o.ProgramId.ToLower().Equals(model.ProgramId.ToLower()));
                if (courseExist != null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0002, TextResourceKey.Course);
                }

                var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
                if (user == null)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
                }

                if (model.StartDate > model.FinishDate)
                {
                    throw NTSException.CreateInstance(MessageResourceKey.MSG0046, TextResourceKey.Course);
                }

                var checkOrder = _sqlContext.Course.Where(b => b.DisplayIndex == model.DisplayIndex).FirstOrDefault();
                Model.Entities.Course course = new Model.Entities.Course();
                string NameOld = string.Empty;
                var nameCourseExist = _sqlContext.Course.AsNoTracking().FirstOrDefault(o => !o.Id.Equals(id) && o.Name.ToLower().Equals(model.Name.ToLower()));
                if (checkOrder != null)
                {
                    var newOrder = _sqlContext.Course.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    NameOld = newOrder.Name;
                    int oldOrder = newOrder.DisplayIndex;
                    if (checkOrder.DisplayIndex < newOrder.DisplayIndex)
                    {
                        var listCourseChange = _sqlContext.Course.Where(a => a.DisplayIndex > checkOrder.DisplayIndex && a.DisplayIndex < newOrder.DisplayIndex);
                        if (listCourseChange.Count() > 0)
                        {
                            foreach (var item in listCourseChange)
                            {
                                item.DisplayIndex++;
                            }

                        }
                        checkOrder.DisplayIndex++;
                    }

                    if (checkOrder.DisplayIndex > newOrder.DisplayIndex)
                    {
                        var listOrderChange = _sqlContext.Course.Where(a => a.DisplayIndex > newOrder.DisplayIndex && a.DisplayIndex < checkOrder.DisplayIndex);
                        if (listOrderChange.ToList().Count() > 0)
                        {
                            foreach (var item in listOrderChange)
                            {
                                item.DisplayIndex--;
                            }
                        }
                        checkOrder.DisplayIndex = checkOrder.DisplayIndex - 1;
                    }
                    newOrder.Name = model.Name;
                    newOrder.Content = model.Content;
                    newOrder.Description = model.Description;
                    newOrder.StartDate = model.StartDate;
                    newOrder.FinishDate = model.FinishDate;
                    newOrder.ImagePath = model.ImagePath;
                    newOrder.ProgramId = model.ProgramId;
                    newOrder.Status = model.Status;
                    newOrder.UpdateBy = userId;
                    newOrder.UpdateDate = DateTime.Now;
                    newOrder.DisplayIndex = model.DisplayIndex;
                    newOrder.CertificateTemplateId = model.CertificateTemplateId;
                    newOrder.RequestDate = DateTime.Now;
                    newOrder.RequestBy = model.RequestBy;
                    newOrder.ApprovalDate = DateTime.Now;
                    newOrder.ApprovalBy = model.ApprovalBy;
                    newOrder.ApprovalStatus = model.ApprovalStatus;
                    newOrder.Slug = nameCourseExist == null ? SlugHelper.ConverNameToSlug(model.Name) : SlugHelper.ConverNameToSlug(model.Name) + DateTime.Now.ToString("yyyyMMddHHmmss");
                    AddLearer(newOrder.Id, model.LearnerCourses);
                    AddEmployee(newOrder.Id, model.EmployeeCourses);
                    AddLesson(newOrder.Id, model.LessonCourses, model.IsDelete);
                    course = newOrder;
                }
                else
                {
                    var newOrder = _sqlContext.Course.Where(r => r.Id.Equals(model.Id)).FirstOrDefault();
                    NameOld = newOrder.Name;

                    var listOrder = (from a in _sqlContext.Course.AsNoTracking()
                                     orderby a.DisplayIndex ascending
                                     select new Elearning.Model.Entities.Course
                                     {
                                         Id = a.Id,
                                         Description = a.Description,
                                         ImagePath = a.ImagePath,
                                         DisplayIndex = a.DisplayIndex,
                                         Status = a.Status,
                                         Content = a.Content,
                                         FinishDate = a.FinishDate,
                                         StartDate = a.StartDate,
                                         Name = a.Name,
                                         ProgramId = a.ProgramId,
                                         CertificateTemplateId = a.CertificateTemplateId
                                     }).AsQueryable();
                    if (newOrder.DisplayIndex == 1 && listOrder.Count() == 1 && !model.DisplayIndex.Equals("1"))
                    {
                        throw new Exception("Không được quyền sửa thứ tự ưu tiên. Vui lòng xem lại!");
                    }
                    newOrder.Name = model.Name;
                    newOrder.Content = model.Content;
                    newOrder.Description = model.Description;
                    newOrder.StartDate = model.StartDate;
                    newOrder.FinishDate = model.FinishDate;
                    newOrder.ImagePath = model.ImagePath;
                    newOrder.ProgramId = model.ProgramId;
                    newOrder.Status = model.Status;
                    newOrder.UpdateBy = userId;
                    newOrder.UpdateDate = DateTime.Now;
                    newOrder.UpdateDate = DateTime.Now;
                    newOrder.UpdateBy = userId;
                    newOrder.RequestDate = DateTime.Now;
                    newOrder.RequestBy = model.RequestBy;
                    newOrder.ManagerUnitId = model.ManagerUnitId;
                    newOrder.ApprovalDate = DateTime.Now;
                    newOrder.ApprovalBy = model.ApprovalBy;
                    newOrder.ApprovalStatus = model.ApprovalStatus;
                    newOrder.DisplayIndex = model.DisplayIndex;
                    newOrder.Slug = nameCourseExist == null ? SlugHelper.ConverNameToSlug(model.Name) : SlugHelper.ConverNameToSlug(model.Name) + DateTime.Now.ToString("yyyyMMddHHmmss");
                    AddLearer(newOrder.Id, model.LearnerCourses);
                    AddEmployee(newOrder.Id, model.EmployeeCourses);
                    AddLesson(newOrder.Id, model.LessonCourses, model.IsDelete);
                    course = newOrder;
                }

                try
                {

                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, userId);
                    string action = string.Empty;
                    if (NameOld.ToLower() == model.Name.ToLower())
                    {
                        userHistory.Content = "Cập nhật chương trình đào tạo tên là: " + NameOld;
                        action = $"Tài khoản {user.UserName} đã cập nhật khóa học: {NameOld}";
                    }
                    else
                    {
                        userHistory.Content = "Cập nhật chương trình đào tạo có tên ban đầu là: " + NameOld + " thành " + model.Name;
                        action = $"Tài khoản {user.UserName} đã cập nhật khóa học: {NameOld} thành {model.Name}";
                    }
                    LogService.Event(userHistory, _detection);


                    // Lưu lịch sử
                    ApprovalHistorys(course, "", action);
                    await _sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }

        }

        public async Task<CourseInfoModel> GetCourseByIdAsync(string id)
        {
            var courseInfo = (from a in _sqlContext.Course.AsNoTracking()
                              where a.Id.Equals(id)
                              select new CourseInfoModel
                              {
                                  Id = a.Id,
                                  Name = a.Name,
                                  Content = a.Content,
                                  ImagePath = a.ImagePath,
                                  Description = a.Description,
                                  Status = a.Status,
                                  StartDate = a.StartDate,
                                  FinishDate = a.FinishDate,
                                  ProgramId = a.ProgramId,
                                  DisplayIndex = a.DisplayIndex,
                                  CertificateTemplateId = a.CertificateTemplateId,
                                  RequestBy = a.RequestBy,
                                  ManagerUnitId = a.ManagerUnitId,
                                  ApprovalBy = a.ApprovalBy,
                                  ApprovalStatus = a.ApprovalStatus,
                              }).FirstOrDefault();

            if (courseInfo == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }
            return courseInfo;
        }

        /// <summary>
        /// Lấy danh sách khóa học cho bài giảng
        /// </summary>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<LessonModel>> GetListLesson(LessonModelSearch modelSearch)
        {
            var data = (from a in _sqlContext.Lesson.AsNoTracking()
                        join b in _sqlContext.Category.AsNoTracking() on a.CategoryId equals b.Id
                        where a.ApprovalStatus == Constants.Course_Approval_Approved && !modelSearch.ListIdSelect.Contains(a.Id)
                        select new LessonModel
                        {
                            LessonId = a.Id,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            Name = a.Name,
                            Index = 0,
                            CategoryId = a.CategoryId,
                            CategoryName = b.Name,
                            Type = a.Type
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(modelSearch.Name))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(modelSearch.Name.ToUpper()));
            }

            if (!string.IsNullOrEmpty(modelSearch.CategoryId))
            {
                data = data.Where(r => r.CategoryId.Equals(modelSearch.CategoryId));
            }

            SearchBaseResultModel<LessonModel> searchResult = new SearchBaseResultModel<LessonModel>();

            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.ToList();

            return searchResult;
        }

        public async Task<SearchBaseResultModel<LessonModel>> SearchLessonByCourseId(string courseId)
        {
            var data = (from a in _sqlContext.LessonCourse.AsNoTracking()
                        where a.CourseId.Equals(courseId)
                        join b in _sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                        orderby a.DisplayIndex
                        select new LessonModel
                        {
                            Id = a.Id,
                            LessonId = b.Id,
                            Description = b.Description,
                            ImagePath = b.ImagePath,
                            Name = b.Name,
                            DisplayIndex = a.DisplayIndex,
                            Type = b.Type
                        }).AsQueryable();

            SearchBaseResultModel<LessonModel> searchResult = new SearchBaseResultModel<LessonModel>();

            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Select(r => r.Id).Count();
            searchResult.DataResults = data.OrderBy(s => s.DisplayIndex).ToList();

            List<LessonFrameModel> list;
            foreach (var item in searchResult.DataResults)
            {
                list = new List<LessonFrameModel>();
                list = (from a in _sqlContext.LessonFrame.AsNoTracking()
                        where a.LessonId.Equals(item.LessonId)
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

                //if (item.Type == Constants.Lesson_Type_Theory)
                //{
                //    item.Time = list.Sum(i => i.EstimatedTime);
                //}

                foreach (var ite in list)
                {
                    if (ite.Type == Constants.Lesson_Type_Study)
                    {
                        var question = (from a in _sqlContext.LessonFrameQuestion.AsNoTracking()
                                        join b in _sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                        join c in _sqlContext.Topic.AsNoTracking() on b.TopicId equals c.Id
                                        where a.LessonFrameId.Equals(ite.Id) && b.Status == Constants.Question_Status_Active
                                        orderby c.Name
                                        select new LessonQuestionModel
                                        {
                                            Id = b.Id,
                                            TopicId = b.TopicId,
                                            TopicName = c.Name,
                                            Content = b.Content,
                                            Type = b.Type
                                        }).ToList();

                        List<AnswerInfoModel> listAnswerMode = new List<AnswerInfoModel>();
                        foreach (var items in question)
                        {
                            listAnswerMode = (from a in _sqlContext.Answer.AsNoTracking()
                                              where a.QuestionId.Equals(items.Id)
                                              orderby a.AnswerLabel
                                              select new AnswerInfoModel
                                              {
                                                  Id = a.Id,
                                                  QuestionId = a.QuestionId,
                                                  AnswerLabel = a.AnswerLabel,
                                                  IsCorrect = false,
                                                  AnswerContent = item.Type == 4 ? "" : a.AnswerContent,
                                                  DisplayIndex = a.DisplayIndex
                                              }).ToList();
                            items.ListAnswer = listAnswerMode.ToList();
                        }

                        ite.ListQuestion = question;
                    }
                }

                item.ListLessonFrame = list;
            }

            return searchResult;
        }

        /// <summary>
        /// Lấy danh sách theo người học
        /// </summary>
        /// <param name="courseId"></param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<LearnerResultModel>> SearchLearner(string courseId)
        {
            var data = (from a in _sqlContext.LearnerCourse.AsNoTracking()
                        where a.CourseId.Equals(courseId)
                        join b in _sqlContext.Learner.AsNoTracking() on a.LearnerId equals b.Id
                        select new LearnerResultModel
                        {
                            Id = b.Id,
                            Name = b.Name,
                            PhoneNumber = b.PhoneNumber,
                            Email = b.Email,
                            Address = b.Address,
                        }).AsQueryable();

            SearchBaseResultModel<LearnerResultModel> searchResult = new SearchBaseResultModel<LearnerResultModel>();
            searchResult.TotalItems = await data.CountAsync();
            searchResult.DataResults = await data.OrderBy(a => a.Name).ToListAsync();

            return searchResult;
        }

        public async Task<SearchBaseResultModel<ProgressModel>> GetProgress(ProgressSearchModel model)
        {
            //Số bài giảng của khóa học
            var lessonCourse = _sqlContext.LessonCourse.AsNoTracking().Where(r => r.CourseId.Equals(model.Id)).ToList();
            int availableCourse = lessonCourse.Count();
            var data = (from a in _sqlContext.LearnerCourse
                        where a.CourseId.Equals(model.Id)
                        join b in _sqlContext.Learner.AsNoTracking() on a.LearnerId equals b.Id
                        join c in _sqlContext.Course.AsNoTracking() on a.CourseId equals c.Id
                        where c.ProgramId == model.ProgramId
                        select new ProgressModel
                        {
                            CourseId = a.CourseId,
                            LearnerId = b.Id,
                            Name = b.Name,
                            Email = b.Email,
                            Phone = b.PhoneNumber,
                            AvailableCourse = availableCourse,
                            RegistrationDate = a.RegistrationDate,
                            Address = b.Address,
                            Province = _sqlContext.Province.FirstOrDefault(s => s.ProvinceId == b.ProvinceId).Name,
                            District = _sqlContext.District.FirstOrDefault(s => s.DistrictId == b.DistrictId).Name,
                            Ward = _sqlContext.Ward.FirstOrDefault(s => s.WardId == b.WardId).Name,
                        }).ToList();

            foreach (var item in data)
            {
                var lessonHistories = _sqlContext.LessonHistory.Where(s => s.LearnerId == item.LearnerId && item.CourseId == s.CourseId);
                item.StudiedCourse = lessonHistories.Count();
                var test = _sqlContext.Test.Where(s => s.LearnerId == item.LearnerId && item.CourseId == s.CourseId).OrderByDescending(s => s.StartDate).FirstOrDefault();
                if (test != null)
                {
                    item.TotalCorrect = test.TotalCorrect;
                    item.TotalQuestion = test.TotalQuestion;
                }
                else
                {
                    item.TotalCorrect = 0;
                    item.TotalQuestion = 0;
                }
                if (lessonHistories != null && lessonHistories.Count() != 0)
                {
                    var lastDate = lessonHistories.OrderByDescending(s => s.StartDate).FirstOrDefault();
                    item.LastDate = lastDate.StartDate;
                }
            }
            SearchBaseResultModel<ProgressModel> searchResult = new SearchBaseResultModel<ProgressModel>();
            //Tổng số bản ghi, 
            searchResult.TotalItems = data.Count();
            searchResult.DataResults = data.OrderBy(s => s.Name).Skip((model.PageNumber - 1) * model.PageSize).Take(model.PageSize).ToList();
            return searchResult;

        }

        /// <summary>
        /// Lấy danh sách các khóa học hdv phụ trách
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<EmployeeCourseModel>> GetEmployeeCourseAsync(string programId, string employeeId)
        {
            List<EmployeeCourseModel> result = new List<EmployeeCourseModel>();
            var user = _sqlContext.User.Where(s => s.ObjectId == employeeId).FirstOrDefault();
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }
            if (user.Type == Constants.User_UserType_Admin)
            {
                result = await (from b in _sqlContext.Course.AsNoTracking()
                                where b.ProgramId == programId
                                select new EmployeeCourseModel
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                }).OrderBy(r => r.Name).ToListAsync();
            }
            else
            {
                result = await (from a in _sqlContext.EmployeeCourse.AsNoTracking()
                                where a.EmployeeId.Equals(employeeId)
                                join b in _sqlContext.Course.AsNoTracking() on a.CourseId equals b.Id
                                where b.ProgramId == programId
                                select new EmployeeCourseModel
                                {
                                    Id = b.Id,
                                    Name = b.Name,
                                }).OrderBy(r => r.Name).ToListAsync();
            }
            return result;
        }

        public async Task<List<TestResultModel>> GetTestResult(string courseId, string learnerId)
        {
            var data = await (from a in _sqlContext.Lesson.AsNoTracking()
                              join b in _sqlContext.LessonCourse.AsNoTracking() on a.Id equals b.LessonId
                              where b.CourseId == courseId
                              select new TestResultModel
                              {
                                  LessonId = a.Id,
                                  LessonName = a.Name,
                                  IsChecked = false,
                                  DisplayIndex = b.DisplayIndex,
                              }).OrderBy(r => r.DisplayIndex).ToListAsync();
            var lessonHistory = _sqlContext.LessonHistory.Where(s => s.CourseId == courseId && s.LearnerId == learnerId).Select(s => s.LessonId).ToList();
            foreach (var item in data)
            {
                int count = 0;
                foreach (var i in lessonHistory)
                {
                    if (item.LessonId == i)
                    {
                        item.IsChecked = true;
                    }
                    else
                    {
                        count++;
                    }
                }
                if (count == lessonHistory.Count)
                {
                    item.IsChecked = false;
                }
                var test = (from a in _sqlContext.Test.AsNoTracking()
                            where a.CourseId == courseId && a.LearnerId == learnerId && a.LessonId == item.LessonId
                            select new ResultModel
                            {
                                TestId = a.Id,
                                TestDate = a.StartDate,
                                TotalCorrect = a.TotalCorrect,
                                TotalQuestion = a.TotalQuestion,
                            }).OrderByDescending(s => s.TestDate).ToList();
                item.Results = test;
            }
            return data;
        }

        public async Task<List<QuestionModel>> GetQuestionByLessonId(string testId)
        {
            var test = _sqlContext.Test.Find(testId);
            if (test == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Test);
            }
            var data = (from a in _sqlContext.Question.AsNoTracking()
                        join b in _sqlContext.LessonQuestion.AsNoTracking()
                        on a.Id equals b.QuestionId
                        join c in _sqlContext.Lesson.AsNoTracking()
                        on b.LessonId equals c.Id
                        join d in _sqlContext.LessonCourse.AsNoTracking()
                        on c.Id equals d.LessonId
                        join e in _sqlContext.LearnerCourse.AsNoTracking()
                        on d.CourseId equals e.CourseId
                        where d.CourseId == test.CourseId && test.LessonId == c.Id && test.LearnerId == e.LearnerId
                        select new QuestionModel
                        {
                            QuestionId = a.Id,
                            Content = a.Content,
                            Type = a.Type,
                            IsCorrect = false,
                        }).ToList();
            foreach (var item in data)
            {
                var answer = (from a in _sqlContext.Answer.AsNoTracking()
                                  //join b  in _sqlContext.AnswerLearner.AsNoTracking() on a.Id equals b.AnswerId
                                  //into ab
                                  //from ba in ab.DefaultIfEmpty()
                              where a.QuestionId == item.QuestionId
                              select new AnswerModel
                              {
                                  AnswerId = a.Id,
                                  AnswerContent = a.AnswerContent,
                                  AnswerLabel = a.AnswerLabel,
                                  DisplayIndex = a.DisplayIndex,
                                  IsCorrect = a.IsCorrect,
                                  IsChecked = false,
                              }).OrderBy(s => s.AnswerLabel).ToList();
                item.Answers = answer;
                var answerLearner = (from a in _sqlContext.AnswerLearner.AsNoTracking()
                                     where a.QuestionId == item.QuestionId && a.TestId == test.Id
                                     select new AnswerLearnerModel
                                     {
                                         AnswerId = a.AnswerId,
                                         IsCorrect = a.IsCorrect,
                                         AnswerContent = a.AnswerContent,
                                         DisplayIndex = a.DisplayIndex
                                     }).ToList();
                item.AnswerLearners = answerLearner;
                var countLearnerAnswer = answerLearner.Count();
                if (item.Type == 1 || item.Type == 2 || item.Type == 3)
                {
                    var answerCorect = answer.Where(s => s.IsCorrect == true).ToList();
                    if (countLearnerAnswer > 0)
                    {
                        int dem = 0;
                        foreach (var a in answer)
                        {
                            foreach (var b in answerLearner)
                            {
                                if (a.AnswerId == b.AnswerId && b.IsCorrect == true)
                                {
                                    a.IsChecked = true;
                                }
                            }
                        }
                        foreach (var a in answerCorect)
                        {
                            foreach (var b in answerLearner.Where(s => s.IsCorrect.Value == true))
                            {
                                if (a.AnswerId == b.AnswerId)
                                {
                                    dem++;
                                }
                            }
                        }
                        if (dem == answerCorect.Count())
                        {
                            item.IsCorrect = true;
                        }
                    }
                }
                else if (item.Type == 4)
                {
                    int dem = 0;
                    if (answer.Count() == answerLearner.Count())
                    {
                        foreach (var i in answer)
                        {
                            var answerContent = answerLearner.FirstOrDefault(s => s.DisplayIndex == i.DisplayIndex).AnswerContent;
                            if (i.AnswerContent.Trim().ToUpper() == answerContent.Trim().ToUpper()) { dem++; }
                        }

                        if (dem == answer.Count())
                        {
                            item.IsCorrect = true;
                        }
                    }
                }
                else if (item.Type == 5)
                {
                    if (answerLearner.Count() == 0)
                    {
                        item.IsCorrect = false;
                    }
                    else
                    {
                        item.AnswerLearners = answerLearner.OrderBy(s => s.DisplayIndex).ToList();
                        item.Answers = answer.OrderBy(s => s.DisplayIndex).ToList();
                        StringBuilder sbAnswer = new StringBuilder();
                        StringBuilder sbAnswerLearner = new StringBuilder();
                        foreach (var i in answer.OrderBy(s => s.DisplayIndex))
                        {
                            sbAnswer.Append(i.AnswerContent);
                        }
                        foreach (var i in answerLearner.OrderBy(s => s.DisplayIndex))
                        {
                            sbAnswerLearner.Append(i.AnswerContent);
                        }
                        if (sbAnswer.Equals(sbAnswerLearner))
                        {
                            item.IsCorrect = true;
                        }
                    }

                }
            }
            return data;
        }

        public async Task<List<FileTemplateModel>> GetFileTemplates()
        {
            var data = await (from a in _sqlContext.FileTemplate.AsNoTracking()
                              where a.Type == true
                              select new FileTemplateModel
                              {
                                  Id = a.Id,
                                  Name = a.Name
                              }).ToListAsync();
            return data;
        }

        public async Task<string> PrintCertificate(CertificateModel model)
        {
            try
            {
                string pathExport = "";
                long timestamp = new DateTimeOffset(DateTime.UtcNow).ToUnixTimeMilliseconds();
                string resultPath = "Export/" + timestamp + ".docx";
                pathExport = Path.Combine(Directory.GetCurrentDirectory(), resultPath);
                WordDocument document;
                WordDocument documentResult = new WordDocument();

                string templatePath = null;
                var template = _sqlContext.FileTemplate.AsNoTracking().Where(r => r.Id.Equals(model.TemplateId)).FirstOrDefault();
                if (template != null)
                {
                    //In chứng  chỉ
                    templatePath = Path.Combine(Directory.GetCurrentDirectory(), template.FilePath);
                    var learnerNames = _sqlContext.Learner.Where(r => model.LearnerIds.Contains(r.Id)).OrderBy(r => r.Name).Select(r => r.Name).ToList();
                    foreach (var name in learnerNames)
                    {
                        document = FillData(templatePath, model.CourseName, name);

                        foreach (IWSection sec in document.Sections)
                        {
                            documentResult.Sections.Add(sec.Clone());
                        }
                        document.Close();
                    }

                    using (var outputStream = new FileStream(pathExport, FileMode.Create))
                    {
                        documentResult.Save(outputStream, FormatType.Docx);
                    }
                    documentResult.Close();

                    //Cập nhật db
                    _sqlContext.LearnerCourse.Where(r => model.LearnerIds.Contains(r.LearnerId) && r.CourseId.Equals(model.CourseId)).ToList().ForEach(r => r.CertificateDatePrint = DateTime.Now);
                    _sqlContext.SaveChanges();
                }
                return resultPath;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public WordDocument FillData(string templatePath, string courseName, string learnerName)
        {
            try
            {
                DateTime dateTime = DateTime.Now;
                FileStream fileStreamPath = new FileStream(templatePath, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                WordDocument document = new WordDocument();
                document = new WordDocument(fileStreamPath, FormatType.Automatic);
                document.Replace("<name>", learnerName.ToUpper(), false, true);
                document.Replace("<course>", courseName.ToUpper(), false, true);
                document.Replace("<dd>", dateTime.Day.ToString(), false, true);
                document.Replace("<mm>", dateTime.Month.ToString(), false, true);
                document.Replace("<yyyy>", dateTime.Year.ToString(), false, true);
                fileStreamPath.Close();
                return document;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<List<CbbOrderStringModel>> GetListOrder()
        {
            List<CbbOrderStringModel> searchResult = new List<CbbOrderStringModel>();
            try
            {
                var ListModel = (from a in _sqlContext.Course.AsNoTracking()
                                 orderby a.DisplayIndex
                                 select new CbbOrderStringModel()
                                 {
                                     Id = a.Id,
                                     Order = a.DisplayIndex,
                                 }).AsQueryable();
                searchResult = ListModel.ToList();
                if (searchResult.Count() == 0)
                {
                    CbbOrderStringModel addFirstIndex = new CbbOrderStringModel();
                    addFirstIndex.Order = 1;
                    searchResult.Add(addFirstIndex);
                }
                else
                {
                    var maxIndex = _sqlContext.Course.AsNoTracking().Select(b => b.DisplayIndex).Max();
                    CbbOrderStringModel addIndex = new CbbOrderStringModel();
                    addIndex.Order = (maxIndex + 1);
                    searchResult.Add(addIndex);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Có lỗi phát sinh. " + ex.Message);
            }

            return searchResult;
        }

        /// <summary>
        /// Yêu cầu duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task RequestCourseAsync(string id, string userId, StatusModel model)
        {
            var course = _sqlContext.Course.FirstOrDefault(i => i.Id.Equals(id));
            if (course == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }

            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = _sqlContext.Database.BeginTransaction())
            {
                course.ApprovalStatus = Constants.Course_Approval_Request;
                course.RequestBy = userId;
                course.RequestDate = DateTime.Now;

                // Lưu lịch sử
                string action = $"Tài khoản {user.UserName} đã yêu cầu duyệt.";
                ApprovalHistorys(course, model.Content, action);

                try
                {
                    await _sqlContext.SaveChangesAsync();
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
        /// Duyệt khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task ApprovalCourseAsync(string id, string userId, StatusModel model)
        {
            var course = _sqlContext.Course.FirstOrDefault(i => i.Id.Equals(id));
            if (course == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }

            var user = _sqlContext.User.FirstOrDefault(i => i.Id.Equals(userId));
            if (user == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.User);
            }

            using (var transaction = _sqlContext.Database.BeginTransaction())
            {
                course.ApprovalStatus = model.Status;
                course.ApprovalBy = userId;
                course.ApprovalDate = DateTime.Now;

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
                ApprovalHistorys(course, model.Content, action);

                try
                {
                    await _sqlContext.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw ex;
                }
            }
        }

        private void ApprovalHistorys(Elearning.Model.Entities.Course course, string content, string action)
        {
            ApprovalHistory approvalHistory = new ApprovalHistory()
            {
                CourseId = course.Id,
                Action = action,
                Content = content,
                ApprovalStatus = course.ApprovalStatus,
                ProcessingDate = DateTime.Now
            };

            _sqlContext.ApprovalHistory.Add(approvalHistory);
        }

        /// <summary>
        /// Danh sách lịch sử
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<ApprovalHistoryModel>> GetListApprovalStatusAsync(string id)
        {
            var data = await (from a in _sqlContext.ApprovalHistory.AsNoTracking()
                              where a.CourseId.Equals(id)
                              orderby a.ProcessingDate descending
                              select new ApprovalHistoryModel
                              {
                                  Id = a.Id,
                                  CourseId = a.CourseId,
                                  Action = a.Action,
                                  Content = a.Content,
                                  ApprovalStatus = a.ApprovalStatus,
                                  ProcessingDate = a.ProcessingDate
                              }).ToListAsync();

            return data;
        }

        /// <summary>
        /// Lấy bài thi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<ExamModel> GetExamByIdAsync(string id)
        {
            // Lấy thông tin bài giảng
            var result = await (from a in _sqlContext.Lesson.AsTracking()
                                where a.Id.Equals(id)
                                join b in _sqlContext.Test.AsNoTracking() on a.Id equals b.LessonId into ab
                                from ba in ab.DefaultIfEmpty()
                                select new ExamModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    IsExam = a.IsExam,
                                    ExamTime = a.ExamTime,
                                    Type = a.Type,
                                    FinishDate = ba.FinishDate,
                                    StartDate = ba.StartDate,
                                    ApprovalStatus = a.ApprovalStatus,
                                }).FirstOrDefaultAsync();
            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (result.FinishDate != null)
            {
                TimeSpan ts = result.FinishDate.Value - result.StartDate;
                result.ExamTime = result.ExamTime - Convert.ToInt32(ts.TotalSeconds);
            }

            // Lấy câu hỏi trong bài giảng
            var question = (from a in _sqlContext.LessonQuestion.AsNoTracking()
                            join b in _sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                            where a.LessonId.Equals(result.Id)
                            orderby b.CreateDate
                            select new QuestionCreateModel
                            {
                                Id = b.Id,
                                Content = b.Content,
                                Type = b.Type
                            }).ToList();

            IEnumerable<AnswerInfoModel> listAnswerMode = new List<AnswerInfoModel>();
            foreach (var item in question)
            {
                listAnswerMode = (from a in _sqlContext.Answer.AsNoTracking()
                                  where a.QuestionId.Equals(item.Id)
                                  orderby a.AnswerLabel
                                  select new AnswerInfoModel
                                  {
                                      Id = a.Id,
                                      QuestionId = a.QuestionId,
                                      AnswerLabel = a.AnswerLabel,
                                      IsCorrect = false,
                                      AnswerContent = item.Type == 4 ? "" : a.AnswerContent,
                                      DisplayIndex = a.DisplayIndex
                                  }).AsQueryable();
                item.ListAnswer = listAnswerMode.ToList();
            }

            // Gán list câu hỏi
            result.ListQuestion = question;

            return result;
        }
    }
}
