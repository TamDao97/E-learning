using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Models.Settings;
using Microsoft.Extensions.Options;
using Elearning.Model.Models.Mobile.Program;
using System;

namespace Elearning.Mobile.Service.Program
{
    public class MobileProgramService : IMobileProgramService
    {

        private readonly ElearningContext sqlContext;
        private readonly AppSettingModel appSettingModel;
        public MobileProgramService(ElearningContext sqlContext, IOptions<AppSettingModel> options)
        {
            this.sqlContext = sqlContext;
            this.appSettingModel = options.Value;
        }
        /// <summary>
        /// Danh sách chương trình học
        /// </summary>
        /// <returns></returns>
        public async Task<List<MobileProgramModel>> GetListProgram(string learnerId)
        {
            var myCourse = from m in sqlContext.LearnerCourse
                           where m.LearnerId == learnerId
                           select new
                           {
                               CourseId = m.CourseId
                           };
            var model = (from a in sqlContext.Program.AsNoTracking()
                         where (a.Status == Constants.Program_Status_Show)
                         select new MobileProgramModel
                         {
                             ProgramId = a.Id,
                             Name = a.Name,
                         }).ToList();
            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            foreach (var item in model)
            {
                var coursesModel = from a in sqlContext.Course.AsNoTracking()
                                   where a.ProgramId.Equals(item.ProgramId) && a.ApprovalStatus == Constants.Course_Approval_Approved && a.Status == Constants.Course_Status_Show
                                   && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate
                                   select new MobileCourseModel()
                                   {
                                       CourseId = a.Id,
                                       Description = a.Description,
                                       Name = a.Name,
                                       CreateDate = a.CreateDate,
                                       StartDate = a.StartDate,
                                       ImagePath = appSettingModel.ServerApiUrl + a.ImagePath,
                                       CommentNumber = sqlContext.Comment.Where(s => s.CourseId == a.Id).Count(),
                                       EmployeeNames = (from e in sqlContext.EmployeeCourse.AsNoTracking()
                                                        where e.CourseId.Equals(a.Id)
                                                        join f in sqlContext.Employee.AsNoTracking()
                                                        on e.EmployeeId equals f.Id
                                                        select new
                                                        {
                                                            EmployeeName = f.Name == null ? "" : f.Name,
                                                        }).Select(s => s.EmployeeName).ToList(),
                                       IsRegister = myCourse.FirstOrDefault(s => s.CourseId == a.Id) == null ? false : true,
                                       ApprovalDate = a.ApprovalDate,
                                       IsNew = ( a.ApprovalDate.HasValue ? (DateTime.Now - a.ApprovalDate).Value.Days : 0)<31?true:false,
                                   };

                item.Courses = coursesModel.OrderByDescending(s => s.ApprovalDate).ToList();
            }
            var listData = model.Where(a => a.Courses.Count > 0).OrderBy(s => s.Name).ToList();

            return listData;
        }

        public async Task<MobileProgramDetailModel> GetProgramDetailById(string id, string learnerId)
        {
            var myCourse = from m in sqlContext.LearnerCourse
                           where m.LearnerId == learnerId
                           select new
                           {
                               CourseId = m.CourseId
                           };
            var model = (from a in sqlContext.Program.AsNoTracking()
                         where a.Id == id
                         select new MobileProgramDetailModel
                         {
                             ProgramId = a.Id,
                             Name = a.Name,
                             Content = a.Content,
                             Description = a.Description,
                         }).FirstOrDefault();
            if (model == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Program);
            }
            var coursesModel = from a in sqlContext.Course.AsNoTracking()
                               where a.ProgramId.Equals(id) && a.Status == Constants.Course_Status_Show && a.ApprovalStatus == Constants.Course_Approval_Approved
                               && a.StartDate <= DateTime.Now && DateTime.Now <= a.FinishDate
                               select new MobileCourseModel()
                               {
                                   CourseId = a.Id,
                                   Description = a.Description,
                                   Name = a.Name,
                                   CreateDate = a.CreateDate,
                                   ImagePath = appSettingModel.ServerApiUrl + a.ImagePath,
                                   CommentNumber = sqlContext.Comment.Where(s => s.CourseId == a.Id).Count(),
                                   EmployeeNames = (from e in sqlContext.EmployeeCourse.AsNoTracking()
                                                    where e.CourseId.Equals(a.Id)
                                                    join f in sqlContext.Employee.AsNoTracking()
                                                    on e.EmployeeId equals f.Id
                                                    select new
                                                    {
                                                        EmployeeName = f.Name == null ? "" : f.Name,
                                                    }).Select(s => s.EmployeeName).ToList(),
                                   IsRegister = myCourse.FirstOrDefault(s => s.CourseId == a.Id) == null ? false : true,
                               };
            model.Courses = coursesModel.ToList();
            return model;
        }
    }
}
