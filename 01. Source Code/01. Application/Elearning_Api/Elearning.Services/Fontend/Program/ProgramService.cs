using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Elearning.Model.Models.Fontend.Client_Program;
using NTS.Common;
using Elearning.Model.Models.Fontend.Course;
using Elearning.Model.Entities;
using NTS.Common.Resource;
using Elearning.Services.Fontend.MyCourse;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Http;
using Wangkanai.Detection.Services;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.UserDevice;
using Elearning.Services.Log;

namespace Elearning.Services.Fontend.Program
{
    public class ProgramService : IProgramService
    {
        private readonly ElearningContext _sqlContext;
        private readonly IMyCourseService myCourseService;
        private IMemoryCache _cache;
        private readonly IDetectionService _detection;


        public ProgramService(ElearningContext sqlContext, IMyCourseService courseService, IDetectionService _detection)
        {
            _sqlContext = sqlContext;
            this.myCourseService = courseService;
            this._detection = _detection;

        }

        public async Task<List<ProgramModel>> GetListProgram(string id)
        {
            var data = (from a in _sqlContext.Program.AsNoTracking()
                        where (a.Status == Constants.Program_Status_Show && a.Id != id)
                        select new ProgramModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Status = a.Status,
                            ImagePath = a.ImagePath,
                            Slug = a.Slug,
                        }).Take(2).ToList();

            return data.ToList();
        }
        public async Task<List<ProgramModel>> GetProgramAsync(string learnerId)
        {
            var myCourse = await myCourseService.GetMyCourse(learnerId);
            var data = (from a in _sqlContext.Program.AsNoTracking()
                        where (a.Status == Constants.Program_Status_Show)
                        select new ProgramModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            Status = a.Status,
                            Slug = a.Slug,
                        }).ToList();

            foreach (var item in data)
            {
                var courses = (from a in _sqlContext.Course.AsNoTracking()
                               where (item.Id == a.ProgramId && a.Status == Constants.Course_Status_Show
                               && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate && a.ApprovalStatus == Constants.Course_Approval_Approved)
                               select new CourseModel
                               {
                                   Id = a.Id,
                                   Name = a.Name,
                                   Description = a.Description,
                                   ImagePath = a.ImagePath,
                                   DisplayIndex = a.DisplayIndex,
                                   NumberOfLearner = _sqlContext.LearnerCourse.Where(s => s.CourseId == a.Id).Count(),
                                   StartDate = a.StartDate,
                                   FinishDate = a.FinishDate,
                                   ApprovalDate = a.ApprovalDate,
                                   DateDiff = a.ApprovalDate.HasValue ? (DateTime.Now - a.ApprovalDate).Value.Days : 0,
                                   NumberOfLesson = _sqlContext.LessonCourse.Where(s => s.CourseId == a.Id).Count(),
                                   NumberOfComment = _sqlContext.Comment.Where(s => s.CourseId == a.Id).Count(),
                                   Slug = a.Slug,
                               }).Where(s => s.StartDate <= DateTime.Now && DateTime.Now <= s.FinishDate)
                              .ToList();


                foreach (var i in courses)
                {
                    int d = 0;
                    var listEmployee = from a in _sqlContext.Employee.AsNoTracking()
                                       join b in _sqlContext.EmployeeCourse.AsNoTracking() on a.Id equals b.EmployeeId
                                       where b.CourseId == i.Id
                                       select a.Name;
                    i.ListEmployeeName = listEmployee.ToList();

                    //i.ListLesson = (from lc in _sqlContext.LessonCourse.AsNoTracking()
                    //                join ls in _sqlContext.Lesson.AsNoTracking() on lc.LessonId equals ls.Id
                    //                where lc.CourseId.Equals(i.Id) && ls.Status
                    //                orderby lc.DisplayIndex ascending
                    //                select new LessonModel
                    //                {
                    //                    Id = ls.Id,
                    //                    LessonCourseId = lc.Id,
                    //                    Name = ls.Name,
                    //                    Description = ls.Description,
                    //                    Content = ls.Content,
                    //                    ImagePath = ls.ImagePath,
                    //                    Type = ls.Type,
                    //                    IsExam = ls.IsExam,
                    //                    ExamTime = ls.ExamTime,
                    //                    DisplayIndex = lc.DisplayIndex
                    //                }).OrderBy(s=>s.DisplayIndex).ToList();

                    foreach (var j in myCourse)
                    {

                        if (i.Id == j.Id)
                        {
                            i.IsLearned = true;
                        }
                        else
                        {
                            d++;
                        }
                    }
                    if (d == myCourse.Count)
                    {
                        i.IsLearned = false;
                    }

                }

                item.ListCourse = courses.OrderByDescending(s => s.ApprovalDate).ToList();

            }

            var listData = data.Where(a => a.ListCourse.Count > 0).OrderBy(s => s.Name).ToList();

            return listData;
        }

        public async Task<CourseModel> GetTop2Course()
        {
            var data = (from a in _sqlContext.Course.AsNoTracking()
                        where (a.Status == Constants.Course_Status_Show && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate && a.ApprovalStatus == Constants.Course_Approval_Approved)
                        select new CourseModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Description = a.Description,
                            ImagePath = a.ImagePath,
                            NumberOfLearner = _sqlContext.LearnerCourse.Where(s => s.CourseId == a.Id).Count(),
                            StartDate = a.StartDate,
                            Slug = a.Slug,
                            FinishDate = a.FinishDate,
                            NumberOfComment = _sqlContext.Comment.Where(s => s.CourseId == a.Id).Count(),
                        }).OrderByDescending(s => s.NumberOfLearner).FirstOrDefault();

            var listEmployee = from a in _sqlContext.Employee.AsNoTracking()
                               join b in _sqlContext.EmployeeCourse.AsNoTracking() on a.Id equals b.EmployeeId
                               where data != null && b.CourseId == data.Id
                               select a.Name;

            data.ListEmployeeName = listEmployee.Take(1).ToList();
            return data;
        }

        public async Task<ProgramModel> GetProgramByIdAsync(SearchProgram searchProgram)
        {
            var myCourse = await myCourseService.GetMyCourse(searchProgram.LearnerId);

            List<CourseModel> listCourse = await (from a in _sqlContext.Course.AsNoTracking()
                                                  join b in _sqlContext.Program.AsNoTracking()
                                                  on a.ProgramId equals (b.Id)
                                                  where b.Slug == searchProgram.Slug && a.Status.Equals(Constants.Course_Status_Show)
                                                  && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate && a.ApprovalStatus == Constants.Course_Approval_Approved
                                                  select new CourseModel
                                                  {
                                                      Id = a.Id,
                                                      Name = a.Name,
                                                      Description = a.Description,
                                                      ImagePath = a.ImagePath,
                                                      FinishDate = a.FinishDate,
                                                      StartDate = a.StartDate,
                                                      Slug = a.Slug
                                                  }).Where(s => s.FinishDate >= DateTime.Now && s.StartDate <= DateTime.Now).ToListAsync();


            //if (listCourse.Count == 0)
            //{
            //    throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            //}

            var employee = _sqlContext.Employee.AsNoTracking().ToList();
            var employeeCourse = _sqlContext.EmployeeCourse.AsNoTracking().ToList();
            var lessonCourse = _sqlContext.LessonCourse.AsNoTracking().ToList();
            var comment = _sqlContext.Comment.AsNoTracking().ToList();
            var learedCourse = _sqlContext.LearnerCourse.AsNoTracking().ToList();

            foreach (var item in listCourse)
            {
                item.NumberOfLearner = learedCourse.Count(r => r.CourseId.Equals(item.Id));
                item.NumberOfComment = comment.Count(r => r.CourseId.Equals(item.Id));
                item.NumberOfLesson = lessonCourse.Count(r => r.CourseId.Equals(item.Id));
                item.ListEmployeeName = (from a in employee
                                         join b in employeeCourse on a.Id equals b.EmployeeId
                                         where b.CourseId == item.Id
                                         select a.Name).Take(2).ToList();

                int d = 0;
                foreach (var j in myCourse)
                {

                    if (item.Id == j.Id)
                    {
                        item.IsLearned = true;
                    }
                    else
                    {
                        d++;
                    }
                }
                if (d == myCourse.Count)
                {
                    item.IsLearned = false;
                }
            }

            ProgramModel program = _sqlContext.Program.AsNoTracking()
                .Select(r => new ProgramModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Content = r.Content,
                    ImagePath = r.ImagePath,
                    ListCourse = listCourse,
                    Slug = r.Slug,
                }).FirstOrDefault(r => r.Slug.Equals(searchProgram.Slug));

            return program;
        }
        public async Task CreateLearnerCourseAsync(HttpRequest request, LearnerCourseCreateModel model)
        {
            var checkLearnerCourse = _sqlContext.LearnerCourse.FirstOrDefault(s => s.CourseId == model.CourseId && s.LearnerId == model.LearnerId);
            if (checkLearnerCourse != null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0037, TextResourceKey.LessonCourse);
            }

            var listHistory = _sqlContext.LessonHistory.Where(i => i.CourseId.Equals(model.CourseId) && i.LearnerId.Equals(model.LearnerId)).ToList();
            if (listHistory.Count > 0)
            {
                _sqlContext.LessonHistory.RemoveRange(listHistory);
            }

            List<LessonFrame> lessonFrames = new List<LessonFrame>();
            var list = _sqlContext.LessonCourse.Where(i => i.CourseId.Equals(model.CourseId)).ToList();
            foreach (var item in list)
            {
                lessonFrames.AddRange(_sqlContext.LessonFrame.Where(i => i.LessonId.Equals(item.LessonId)));
            }

            List<LessonFrameHistory> lessonFrameHistories = new List<LessonFrameHistory>();
            List<LessonAnswerLearner> lessonAnswerLearners = new List<LessonAnswerLearner>();
            foreach (var item in lessonFrames)
            {
                lessonFrameHistories.AddRange(_sqlContext.LessonFrameHistory.Where(i => i.LessonFrameId.Equals(item.Id) && i.LearnerId.Equals(model.LearnerId)));
            }

            foreach (var item in lessonFrameHistories)
            {
                lessonAnswerLearners.AddRange(_sqlContext.LessonAnswerLearner.Where(i => i.LessonFrameHistoryId.Equals(item.Id)));
            }
            _sqlContext.LessonAnswerLearner.RemoveRange(lessonAnswerLearners);
            _sqlContext.LessonFrameHistory.RemoveRange(lessonFrameHistories);

            LearnerCourse learnerCourse = new LearnerCourse()
            {
                Id = Guid.NewGuid().ToString(),
                CourseId = model.CourseId,
                LearnerId = model.LearnerId,
                RegistrationDate = DateTime.Now,
            };
            _sqlContext.LearnerCourse.Add(learnerCourse);
            _sqlContext.SaveChanges();

            var courseName = _sqlContext.Course.FirstOrDefault(a => a.Id.Equals(model.CourseId)).Name;
            var learnerName = _sqlContext.Learner.FirstOrDefault(a => a.Id.Equals(model.LearnerId)).Name;

            UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, model.LearnerId);
            userHistory.Content = learnerName + " đã đăng ký khóa học: " + courseName;
            LogService.Event(userHistory, _detection);
        }

    }
}
