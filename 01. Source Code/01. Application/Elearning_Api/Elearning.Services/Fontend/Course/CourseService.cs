using Elearning.Model.Models.Fontend.Client_Program;
using Elearning.Model.Models.Fontend.Course;
using Elearning.Models.Entities;
using Elearning.Services.Fontend.MyCourse;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Resource;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.Course
{
    public class CourseService : ICourseService
    {
        private readonly ElearningContext sQlContext;
        private readonly IMyCourseService myCourseService;

        public CourseService (ElearningContext sQlContext, IMyCourseService courseService)
        {
            this.sQlContext = sQlContext;
            this.myCourseService = courseService;
        }

        /// <summary>
        /// Lấy chi tiết khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<CourseDetailModel> GetCourseById (CourseIdModel courseIdModel)
        {
            var myCourse = await myCourseService.GetMyCourse(courseIdModel.LearnerId);
            CourseDetailModel model = new CourseDetailModel();
            model = await sQlContext.Course.AsNoTracking()
               .Where(r => r.Slug.Equals(courseIdModel.Slug))
               .Select(r => new CourseDetailModel
               {
                   Id = r.Id,
                   ProgramId = r.ProgramId,
                   Name = r.Name,
                   Description = r.Description,
                   Content = r.Content,
                   ImagePath = r.ImagePath,
                   Status = r.Status,
                   StartDate = r.StartDate,
                   Slug=r.Slug,
                   FinishDate = r.FinishDate
               }).FirstOrDefaultAsync();

            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Course);
            }

            model.ListLesson = (from lc in sQlContext.LessonCourse.AsNoTracking()
                                join ls in sQlContext.Lesson.AsNoTracking() on lc.LessonId equals ls.Id
                                where lc.CourseId.Equals(model.Id) && ls.Status
                                orderby lc.DisplayIndex ascending
                                select new LessonModel
                                {
                                    Id = ls.Id,
                                    LessonCourseId = lc.Id,
                                    Name = ls.Name,
                                    Slug=ls.Slug,
                                    Description = ls.Description,
                                    Content = ls.Content,
                                    ImagePath = ls.ImagePath,
                                    Type = ls.Type,
                                    IsExam = ls.IsExam,
                                    ExamTime = ls.ExamTime,
                                    DisplayIndex = lc.DisplayIndex
                                }).ToList();

            model.ListEmployee = (from ec in sQlContext.EmployeeCourse.AsNoTracking()
                                  join em in sQlContext.Employee.AsNoTracking() on ec.EmployeeId equals em.Id
                                  where ec.CourseId.Equals(model.Id)
                                  select new EmployeeModel
                                  {
                                      Id = em.Id,
                                      Name = em.Name,
                                      Gender = em.Gender,
                                      Birthday = em.Birthday,
                                      PhoneNumber = em.PhoneNumber,
                                      Email = em.Email,
                                      Address = em.Address,
                                      Avatar = em.Avatar,
                                      WorkUnit = em.WorkUnit,
                                      Description = em.Description,
                                  }).ToList();

            foreach (var item in model.ListEmployee)
            {
                item.NumberOfCourse = sQlContext.EmployeeCourse.Where(a => a.EmployeeId.Equals(item.Id)).Count();
                item.NumberOfLeared = (from a in sQlContext.LearnerCourse.AsNoTracking()
                                       join b in sQlContext.Learner.AsNoTracking() on a.LearnerId equals b.Id
                                       where a.CourseId.Equals(courseIdModel.Slug)
                                       select b.Id).Count();
            }

            model.TotalLearnerCourse = sQlContext.LearnerCourse.AsNoTracking().Count(r => r.CourseId.Equals(courseIdModel.Slug));

            var listRelatedCourse = (sQlContext.Course.AsNoTracking()
                .Where(r => r.ProgramId.Equals(model.ProgramId) && !r.Slug.Equals(courseIdModel.Slug) && r.Status == true && r.StartDate <= DateTime.Now && DateTime.Now <= r.FinishDate && r.ApprovalStatus == Constants.Course_Approval_Approved)
                .OrderBy(r => r.CreateDate)
                .Take(4).Select(r => new CourseModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Slug=r.Slug,
                    Description = r.Description,
                    ImagePath = r.ImagePath,
                    FinishDate = r.FinishDate,
                    StartDate = r.StartDate,
                })).Where(s => s.FinishDate >= DateTime.Now && s.StartDate <= DateTime.Now).ToList();

            var employee = sQlContext.Employee.AsNoTracking().ToList();
            var employeeCourse = sQlContext.EmployeeCourse.AsNoTracking().ToList();
            var lessonCourse = sQlContext.LessonCourse.AsNoTracking().ToList();
            var learedCourse = sQlContext.LearnerCourse.AsNoTracking().ToList();

            foreach (var item in listRelatedCourse)
            {
                int d = 0;
                item.NumberOfLesson = lessonCourse.Count(r => r.CourseId.Equals(item.Id));
                item.NumberOfLearner = learedCourse.Count(r => r.CourseId.Equals(item.Id));
                item.ListEmployeeName = (from a in employee
                                         join b in employeeCourse on a.Id equals b.EmployeeId
                                         where b.CourseId == item.Id
                                         select a.Name).Take(2).ToList();
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

            model.ListRelatedCourse = listRelatedCourse;

            return model;
        }

        public async Task<List<CourseSearchFrontendModel>> SearchCourseAsync (string learnerId, string searchValue)
        {
            var myCourse = await myCourseService.GetMyCourse(learnerId);
            var courses = (from a in sQlContext.Course.AsNoTracking()
                           where a.Status == true && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate && a.ApprovalStatus == Constants.Course_Approval_Approved
                           join b in sQlContext.Program.AsNoTracking()
                           on a.ProgramId equals b.Id
                           select new CourseSearchFrontendModel
                           {
                               Id = a.Id,
                               Description = a.Description,
                               DisplayIndex = a.DisplayIndex,
                               FinishDate = a.FinishDate,
                               ImagePath = a.ImagePath,
                               Name = a.Name,
                               StartDate = a.StartDate,
                               ProgramName=b.Name,
                               Slug=a.Slug,
                               ProgramDescription=b.Description,
                               NumberOfLearner = sQlContext.LearnerCourse.Where(s => s.CourseId == a.Id).Count(),
                               NumberOfLesson = sQlContext.LessonCourse.Where(s => s.CourseId == a.Id).Count(),
                           }).ToList();
            if(searchValue!=null)
            {
                courses = courses.Where(a => a.Name.Trim().ToUpper().Contains(searchValue.Trim().ToUpper()) ||
                a.Description.Trim().ToUpper().Contains(searchValue.Trim().ToUpper())|| 
                a.ProgramDescription.Trim().ToUpper().Contains(searchValue.Trim().ToUpper())||
                a.ProgramName.Trim().ToUpper().Contains(searchValue.Trim().ToUpper())).ToList();
            }    
            foreach (var i in courses)
            {
                int d = 0;
                var listEmployee = from a in sQlContext.Employee.AsNoTracking()
                                   join b in sQlContext.EmployeeCourse.AsNoTracking() on a.Id equals b.EmployeeId
                                   where b.CourseId == i.Id
                                   select a.Name;
                i.ListEmployeeName = listEmployee.ToList();


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

            return courses;
        }
    }
}
