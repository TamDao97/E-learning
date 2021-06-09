using Elearning.Model.Models.Fontend.Client_Program;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Fontend.Course;
using NTS.Common;

namespace Elearning.Services.Fontend.MyCourse
{
    public class MyCourseService : IMyCourseService
    {
        private readonly ElearningContext _sqlContext;

        public MyCourseService(ElearningContext sqlContext)
        {
            _sqlContext = sqlContext;
        }
        public async Task<List<CourseModel>> GetMyCourse(string learnerId)
        {
            var data = (from a in _sqlContext.LearnerCourse.AsNoTracking()
                        join b in _sqlContext.Learner.AsNoTracking() on a.LearnerId equals (b.Id)
                        join c in _sqlContext.Course.AsNoTracking() on a.CourseId equals (c.Id)
                        where a.LearnerId == learnerId
                        select new CourseModel
                        {
                            Id = a.CourseId,
                            Name = c.Name,
                            Description = c.Description,
                            ImagePath = c.ImagePath,
                            DisplayIndex = c.DisplayIndex,
                            NumberOfLearner = _sqlContext.LearnerCourse.Where(s => s.CourseId == a.CourseId).Count(),
                            StartDate = c.StartDate,
                            NumberOfComment = _sqlContext.Comment.Where(s => s.CourseId == c.Id).Count(),
                            LessonCourseLearned = _sqlContext.LessonHistory.Where(s => s.CourseId == a.CourseId && s.LearnerId == learnerId).Count(),
                            NumberOfLesson = _sqlContext.LessonCourse.Where(s => s.CourseId == c.Id).Count(),
                            Slug = c.Slug,
                        }).ToList();
            foreach (var i in data)
            {
                var list = await (from a in _sqlContext.LessonCourse.AsNoTracking()
                                  join b in _sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                  where b.Type == Constants.Lesson_Type_Theory && a.CourseId.Equals(i.Id)
                                  select new { b.Id }).ToListAsync();

                foreach (var item in list)
                {
                    var totalLessonFrame = _sqlContext.LessonFrame.Where(i => i.LessonId.Equals(item.Id)).Count();
                    var totalLessonFrameHistory = _sqlContext.LessonFrameHistory.Where(u => u.LessonId.Equals(item.Id) && u.CourseId.Equals(i.Id) && u.LearnerId.Equals(learnerId)).Count();

                    if (totalLessonFrame == totalLessonFrameHistory && totalLessonFrame > 0)
                    {
                        i.LessonCourseLearned++;
                    }
                }

                //var listTest = _sqlContext.LessonHistory.Where(s => s.CourseId == i.Id && s.LearnerId == learnerId).ToList();
                var listEmployee = from a in _sqlContext.Employee.AsNoTracking()
                                   join b in _sqlContext.EmployeeCourse.AsNoTracking() on a.Id equals b.EmployeeId
                                   where b.CourseId == i.Id
                                   select a.Name;
                i.ListEmployeeName = listEmployee.Take(2).ToList();
                if (i.NumberOfLesson != 0)
                {
                    i.PercentLearned = i.LessonCourseLearned * 100 / i.NumberOfLesson;
                }
                else
                {
                    i.PercentLearned = 100;
                }

                //i.ListLesson = (from lc in _sqlContext.LessonCourse.AsNoTracking()
                //                    join ls in _sqlContext.Lesson.AsNoTracking() on lc.LessonId equals ls.Id
                //                    where lc.CourseId.Equals(i.Id) && ls.Status
                //                    orderby lc.DisplayIndex ascending
                //                    select new LessonModel
                //                    {
                //                        Id = ls.Id,
                //                        LessonCourseId = lc.Id,
                //                        Name = ls.Name,
                //                        Description = ls.Description,
                //                        Content = ls.Content,
                //                        ImagePath = ls.ImagePath,
                //                        Type = ls.Type,
                //                        IsExam = ls.IsExam,
                //                        ExamTime = ls.ExamTime,
                //                        Slug=ls.Slug,
                //                        DisplayIndex = lc.DisplayIndex
                //                    }).OrderBy(s=>s.DisplayIndex).ToList();
            }
            return data;
        }
    }
}
