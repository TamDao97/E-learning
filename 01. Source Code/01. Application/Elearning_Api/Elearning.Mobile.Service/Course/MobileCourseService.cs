
using Elearning.Models.Base;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Elearning.Model.MobileCourse;
using Elearning.Model.Models.MobileCourse;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Models.Settings;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Http;
using Elearning.Model.Models.Mobile.Course;
using Elearning.Model.Entities;

namespace Elearning.Services.Mobile.Course
{
    public class MobileCourseService : IMobileCourseService
    {
        private readonly ElearningContext _sqlContext;
        private readonly AppSettingModel appSettingModel;

        public MobileCourseService(ElearningContext sqlContext, IOptions<AppSettingModel> options)
        {
            this._sqlContext = sqlContext;
            this.appSettingModel = options.Value;
        }

        /// <summary>
        /// Lấy danh sách khóa học theo id chương trình đào tạo
        /// </summary>
        /// <param name="id">id chương trình đào tạo</param>
        /// <returns></returns>
        public async Task<List<MobileCourseResultModel>> SearchCourseByIdProgram(string id, string learnerId)
        {
            var myCourse = from m in _sqlContext.LearnerCourse
                           where m.LearnerId == learnerId
                           select new
                           {
                               CourseId = m.CourseId
                           };
            var courses = (from a in _sqlContext.Course.AsNoTracking()
                           where a.ProgramId.Equals(id) && a.Status == Constants.Course_Status_Show && a.ApprovalStatus == Constants.Course_Approval_Approved
                           && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate
                           select new MobileCourseResultModel
                           {
                               Id = a.Id,
                               Description = a.Description,
                               ImagePath = appSettingModel.ServerApiUrl + a.ImagePath,
                               StartDate = a.StartDate,
                               Title = a.Name,
                               FinishDate = a.FinishDate,
                               IsRegister = myCourse.FirstOrDefault(s => s.CourseId == a.Id) == null ? false : true,
                           }).ToList();

            var comment = _sqlContext.Comment.AsNoTracking().ToList();
            var employee = _sqlContext.Employee.AsNoTracking().ToList();
            var employeeCourse = _sqlContext.EmployeeCourse.AsNoTracking().ToList();

            foreach (var item in courses)
            {
                item.TotalComment = comment.Count(r => r.CourseId.Equals(item.Id));
                item.ListEmployees = (from a in employee
                                      join b in employeeCourse on a.Id equals b.EmployeeId
                                      where b.CourseId == item.Id
                                      select a.Name).ToList();
            }

            return courses;
        }

        /// <summary>
        /// Lấy chi tiết khóa học theo id
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        public async Task<MobileCourseDetailModel> GetInfoCourseById(string id, string learnerId)
        {
            var courseDetail = (from a in _sqlContext.Course.AsNoTracking()
                                where a.Id.Equals(id) && a.Status == Constants.Course_Status_Show && a.ApprovalStatus == Constants.Course_Approval_Approved
                                && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate
                                select new MobileCourseDetailModel
                                {
                                    Id = a.Id,
                                    Content = a.Content,
                                    Description = a.Description,
                                    Title = a.Name,
                                    isRegister = false,
                                }).FirstOrDefault();

            if (learnerId != null)
            {
                var courseLearner = _sqlContext.LearnerCourse.AsNoTracking().Where(a => a.LearnerId.Equals(learnerId)).Select(r => r.CourseId).ToList();
                var isregeister = courseLearner.Where(a => a.Equals(courseDetail.Id)).Any();
                if (isregeister)
                {
                    courseDetail.isRegister = true;
                }
            }

            if (courseDetail == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }

            return courseDetail;
        }

        public async Task RegisterCourseAsync(MobileLearnerCourseCreateModel model)
        {
            var checkLearnerCourse = _sqlContext.LearnerCourse.FirstOrDefault(s => s.CourseId == model.CourseId && s.LearnerId == model.LearnerId);
            if (checkLearnerCourse != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0037, TextResourceKey.LessonCourse);
            }
            LearnerCourse learnerCourse = new LearnerCourse()
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = model.CourseId,
                LearnerId = model.LearnerId,
                RegistrationDate = DateTime.Now,
            };
            _sqlContext.LearnerCourse.Add(learnerCourse);
            _sqlContext.SaveChanges();
        }

        public async Task<List<MobileCourseSearchResultModel>> SearchCourse(MobileCourseSearchModel model)
        {
            var myCourse = from m in _sqlContext.LearnerCourse
                           where m.LearnerId == model.LearnerId
                           select new
                           {
                               CourseId = m.CourseId
                           };
            var data = (from a in _sqlContext.Course.AsNoTracking()
                        where a.Status == true && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate && a.ApprovalStatus == Constants.Course_Approval_Approved
                        select new MobileCourseSearchResultModel
                        {
                            CourseId = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            ImagePath = appSettingModel.ServerApiUrl + a.ImagePath,
                            CreateDate = a.CreateDate,
                            CommentNumber = _sqlContext.Comment.Where(s => s.CourseId == a.Id).Count(),
                            EmployeeNames = (from e in _sqlContext.EmployeeCourse.AsNoTracking()
                                             where e.CourseId.Equals(a.Id)
                                             join f in _sqlContext.Employee.AsNoTracking()
                                             on e.EmployeeId equals f.Id
                                             select new
                                             {
                                                 EmployeeName = f.Name == null ? "" : f.Name,
                                             }).Select(s => s.EmployeeName).ToList(),
                            IsRegister = myCourse.FirstOrDefault(s => s.CourseId == a.Id) == null ? false : true,
                        }).AsQueryable();

            if (!string.IsNullOrEmpty(model.SearchCondition))
            {
                data = data.Where(r => r.Name.ToUpper().Contains(model.SearchCondition.ToUpper()) || r.Description.ToUpper().Contains(model.SearchCondition.ToUpper()));
            }
            return data.ToList();
        }

        public async Task<MobileMyCourseModel> MyCourse(string learnerId)
        {
            var data = (from a in _sqlContext.LearnerCourse.AsNoTracking()
                        where a.LearnerId == learnerId
                        join b in _sqlContext.Learner.AsNoTracking() on a.LearnerId equals (b.Id)
                        join c in _sqlContext.Course.AsNoTracking() on a.CourseId equals (c.Id)
                        select new MobileMyCourseInfoModel
                        {
                            CourseId = a.CourseId,
                            Name = c.Name,
                            Description = c.Description,
                            CreateDate = c.CreateDate,
                            ImagePath = appSettingModel.ServerApiUrl + c.ImagePath,
                            CommentNumber = _sqlContext.Comment.Where(s => s.CourseId == c.Id).Count(),
                            Completed = _sqlContext.LessonHistory.Where(s => s.CourseId == a.CourseId && s.LearnerId == learnerId).Count(),
                            TotalUnits = _sqlContext.LessonCourse.Where(s => s.CourseId == c.Id).Count(),
                            EmployeeNames = (from e in _sqlContext.EmployeeCourse.AsNoTracking()
                                             where e.CourseId.Equals(a.CourseId)
                                             join f in _sqlContext.Employee.AsNoTracking()
                                             on e.EmployeeId equals f.Id
                                             select new
                                             {
                                                 EmployeeName = f.Name == null ? "" : f.Name,
                                             }).Select(s => s.EmployeeName).ToList(),
                        }).ToList();
            foreach (var item in data)
            {
                var list = await (from a in _sqlContext.LessonCourse.AsNoTracking()
                                  join b in _sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                  where b.Type == Constants.Lesson_Type_Theory && a.CourseId.Equals(item.CourseId)
                                  select new { b.Id }).ToListAsync();

                foreach (var ite in list)
                {
                    var totalLessonFrame = _sqlContext.LessonFrame.Where(i => i.LessonId.Equals(ite.Id)).Count();
                    var totalLessonFrameHistory = _sqlContext.LessonFrameHistory.Where(u => u.LessonId.Equals(ite.Id) && u.CourseId.Equals(item.CourseId) && u.LearnerId.Equals(learnerId)).Count();

                    if (totalLessonFrame == totalLessonFrameHistory && totalLessonFrame > 0)
                    {
                        item.Completed++;
                    }
                }

                if (item.TotalUnits != 0)
                {
                    item.Percent = item.Completed * 100 / item.TotalUnits;
                }
                else
                {
                    item.Percent = 0;
                }
            }

            MobileMyCourseModel myCourseModel = new MobileMyCourseModel()
            {
                Courses = data,
                TotalCourse = data.Count(),
                Completed = (data.Where(s => s.TotalUnits == s.Completed)).Count() + "/" + data.Count(),
                TestComplete = (from a in _sqlContext.Test.AsNoTracking()
                                where a.LearnerId == learnerId
                                join b in _sqlContext.Lesson.AsNoTracking()
                                on a.LessonId equals b.Id
                                where b.Type == Constants.Lesson_Type_Exam
                                select new { a.Id }).Count() + "/" + data.Count(),
            };
            return myCourseModel;
        }

        /// <summary>
        /// Danh sách hướng dẫn viên 
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        public async Task<List<MobileEmployeeCourseModel>> GetListEmployeeCourse(string id)
        {
            var employeeCourses = (from a in _sqlContext.EmployeeCourse.AsNoTracking()
                                   where a.CourseId.Equals(id)
                                   join b in _sqlContext.Employee.AsNoTracking() on a.EmployeeId equals b.Id
                                   select new MobileEmployeeCourseModel
                                   {
                                       EmployeeId = b.Id,
                                       EmployeeName = b.Name,
                                       ImagePath = appSettingModel.ServerApiUrl + b.Avatar,
                                   }).ToList();

            foreach (var item in employeeCourses)
            {
                item.TotalCourse = _sqlContext.EmployeeCourse.Where(a => a.EmployeeId.Equals(item.EmployeeId)).Count();
            }

            return employeeCourses;
        }
    }
}
