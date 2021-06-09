using Elearning.Model.Models.Dashboard;
using Elearning.Model.Models.ReportLearner;
using Elearning.Model.Models.ReportLearnerProvince;
using Elearning.Models.Entities;
using Elearning.Services.ReportLearnerProvince;
using Microsoft.EntityFrameworkCore;
using NTS.Common;
using NTS.Common.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Dashboard
{
    public class DashboardService : IDashboardService
    {
        private readonly ElearningContext sqlContext;
        private readonly IReportLearnerProvinceService reportLearnerProvinceService;

        public DashboardService (ElearningContext sqlContext, IReportLearnerProvinceService reportLearnerProvinceService)
        {
            this.sqlContext = sqlContext;
            this.reportLearnerProvinceService = reportLearnerProvinceService;
        }

        /// <summary>
        /// Thông tin trong năm
        /// </summary>
        /// <returns></returns>
        public async Task<object> GetTotalAsync ()
        {
            double totalLearner = 0;
            double totalLearnerStatus = 0;
            double totalRegister = 0;
            double totalHistoryLogin = 0;
            List<DashboardCourseModel> listCourse = new List<DashboardCourseModel>();
            List<DashboardCourseTopModel> listCourseTop = new List<DashboardCourseTopModel>();

            var list = await sqlContext.LearnerCourse.Where(i => i.RegistrationDate.Year == DateTime.Now.Year).ToListAsync();
            // Tổng số người học
            totalLearner = list.GroupBy(i => i.LearnerId).Count();
            // Tổng số người đang học
            totalLearnerStatus = (from a in list.AsEnumerable()
                                  join b in sqlContext.Course.AsEnumerable() on a.CourseId equals b.Id
                                  where b.FinishDate >= DateTime.Now
                                  select a.LearnerId).GroupBy(i => i).Count();
            // Số tài khoản đăng ký trong năm
            totalRegister = sqlContext.Learner.Where(i => i.CreateDate.Year == DateTime.Now.Year).Count();
            // Số đăng nhập trong năm

            // Khóa học mới
            var listLearner = sqlContext.LearnerCourse.ToList();
            var listLesson = sqlContext.LessonCourse.ToList();
            var courses = (from a in sqlContext.Course.AsNoTracking()
                           join b in sqlContext.Program.AsNoTracking() on a.ProgramId equals b.Id
                           where a.Status == true
                           orderby a.StartDate descending
                           select new DashboardCourseModel
                           {
                               Id = a.Id,
                               Name = a.Name,
                               ProgramName = b.Name,
                               Description = a.Description,
                               ImagePath = a.ImagePath,
                               StartDate = a.StartDate,
                           }).Take(4).ToList();

            foreach (var item in courses)
            {
                item.TotalLearner = listLearner.Where(i => i.CourseId.Equals(item.Id)).Count();
                item.TotalLesson = listLesson.Where(i => i.CourseId.Equals(item.Id)).GroupBy(i => i.CourseId).Count();
            }

            listCourse = courses;

            // Top khóa học
            listCourseTop = (from a in sqlContext.Course.AsNoTracking()
                                 where (a.Status == Constants.Course_Status_Show)
                                 select new DashboardCourseTopModel
                                 {
                                     Id = a.Id,
                                     Name = a.Name,
                                     Description = a.Description,
                                     ImagePath = a.ImagePath,
                                     TotalLearner = sqlContext.LearnerCourse.Where(s => s.CourseId == a.Id).Count(),
                                     StartDate = a.StartDate,
                                 }).OrderByDescending(s => s.TotalLearner).Take(4).ToList();

            var historyLogin = (from a in sqlContext.UserHistories.AsNoTracking()
                                 where a.CreateDate.Year == DateTime.Now.Year
                                 group a by a.UserId into g select new {Id=g.Key});

            totalHistoryLogin = historyLogin.Count();
            return new
            {
                totalLearner,
                totalLearnerStatus,
                totalRegister,
                totalHistoryLogin,
                listCourse,
                listCourseTop
            };
        }

        /// <summary>
        /// Biểu đồ đăng ký khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> GetRegisterCourse (ReportLearnerSearchConditionModel model)
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }

            var list = await sqlContext.LearnerCourse.Where(i => i.RegistrationDate >= dateFrom && i.RegistrationDate <= dateTo).ToListAsync();
            List<ReportLearnerModel> listResult = new List<ReportLearnerModel>();
            List<string> listLable = new List<string>();
            List<double> listData = new List<double>();
            double total = 0;

            if (model.TimeType.Equals(Constants.TimeType_ThisWeek) || model.TimeType.Equals(Constants.TimeType_LastWeek) || model.TimeType.Equals(Constants.TimeType_SevenDay))
            {
                for (int a = 0; a < 7; a++)
                {
                    total = list.Where(i => i.RegistrationDate.Day == dateFrom.Day && i.RegistrationDate.Month == dateFrom.Month).GroupBy(i => i.LearnerId).Count();
                    listLable.Add($"{dateFrom.Day}/{dateFrom.Month}");
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = $"{dateFrom.Day}/{dateFrom.Month}",
                        Total = total
                    });
                    dateFrom = dateFrom.AddDays(1);
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_ThisMonth) || model.TimeType.Equals(Constants.TimeType_LastMonth) || model.TimeType.Equals(Constants.TimeType_Month))
            {
                int day = DateTime.DaysInMonth(dateFrom.Year, dateFrom.Month);
                for (int a = 1; a <= day; a++)
                {
                    total = list.Where(i => i.RegistrationDate.Day == a).GroupBy(i => i.LearnerId).Count();
                    listLable.Add(a.ToString());
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = a.ToString(),
                        Total = total
                    });
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_Quarter))
            {
                if (quarterData == 1)
                {
                    for (int a = 1; a < 4; a++)
                    {
                        total = list.Where(i => i.RegistrationDate.Month == a).GroupBy(i => i.LearnerId).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 2)
                {
                    for (int a = 4; a < 7; a++)
                    {
                        total = list.Where(i => i.RegistrationDate.Month == a).GroupBy(i => i.LearnerId).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 3)
                {
                    for (int a = 7; a < 10; a++)
                    {
                        total = list.Where(i => i.RegistrationDate.Month == a).GroupBy(i => i.LearnerId).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
                else if (quarterData == 4)
                {
                    for (int a = 10; a < 13; a++)
                    {
                        total = list.Where(i => i.RegistrationDate.Month == a).GroupBy(i => i.LearnerId).Count();
                        listLable.Add(a.ToString());
                        listData.Add(total);
                        listResult.Add(new ReportLearnerModel
                        {
                            Name = a.ToString(),
                            Total = total
                        });
                    }
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_ThisYear) || model.TimeType.Equals(Constants.TimeType_LastYear) || model.TimeType.Equals(Constants.TimeType_Year))
            {
                for (int a = 1; a <= 12; a++)
                {
                    total = list.Where(i => i.RegistrationDate.Month == a).GroupBy(i => i.LearnerId).Count();
                    listLable.Add(a.ToString());
                    listData.Add(total);
                    listResult.Add(new ReportLearnerModel
                    {
                        Name = a.ToString(),
                        Total = total
                    });
                }
            }

            return new
            {
                listLable,
                listData
            };
        }

        /// <summary>
        /// Biểu đồ loại đăng ký
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> GetRegister (ReportLearnerSearchConditionModel model)
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }

            var list = await sqlContext.Learner.Where(i => i.CreateDate >= dateFrom && i.CreateDate <= dateTo).ToListAsync();

            List<string> listLable = new List<string>();
            List<double> listGoogle = new List<double>();
            List<double> listFacebook = new List<double>();
            List<double> listEmail = new List<double>();
            double totalGoogle = 0;
            double totalFacebook = 0;
            double totalEmail = 0;

            if (model.TimeType.Equals(Constants.TimeType_ThisMonth) || model.TimeType.Equals(Constants.TimeType_LastMonth) || model.TimeType.Equals(Constants.TimeType_Month))
            {
                int day = DateTime.DaysInMonth(dateFrom.Year, dateFrom.Month);
                for (int a = 1; a <= day; a++)
                {
                    totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Day == a).Count();
                    totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Day == a).Count();
                    totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Day == a).Count();
                    listLable.Add(a.ToString());
                    listGoogle.Add(totalGoogle);
                    listFacebook.Add(totalFacebook);
                    listEmail.Add(totalEmail);
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_Quarter))
            {
                if (quarterData == 1)
                {
                    for (int a = 1; a < 4; a++)
                    {
                        totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Month == a).Count();
                        totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Month == a).Count();
                        totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Month == a).Count();

                        listLable.Add(a.ToString());
                        listGoogle.Add(totalGoogle);
                        listFacebook.Add(totalFacebook);
                        listEmail.Add(totalEmail);

                    }
                }
                else if (quarterData == 2)
                {
                    for (int a = 4; a < 7; a++)
                    {
                        totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Month == a).Count();
                        totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Month == a).Count();
                        totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Month == a).Count();

                        listLable.Add(a.ToString());
                        listGoogle.Add(totalGoogle);
                        listFacebook.Add(totalFacebook);
                        listEmail.Add(totalEmail);
                    }
                }
                else if (quarterData == 3)
                {
                    for (int a = 7; a < 10; a++)
                    {
                        totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Month == a).Count();
                        totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Month == a).Count();
                        totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Month == a).Count();

                        listLable.Add(a.ToString());
                        listGoogle.Add(totalGoogle);
                        listFacebook.Add(totalFacebook);
                        listEmail.Add(totalEmail);
                    }
                }
                else if (quarterData == 4)
                {
                    for (int a = 10; a < 13; a++)
                    {
                        totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Month == a).Count();
                        totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Month == a).Count();
                        totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Month == a).Count();

                        listLable.Add(a.ToString());
                        listGoogle.Add(totalGoogle);
                        listFacebook.Add(totalFacebook);
                        listEmail.Add(totalEmail);
                    }
                }
            }
            else if (model.TimeType.Equals(Constants.TimeType_ThisYear) || model.TimeType.Equals(Constants.TimeType_LastYear) || model.TimeType.Equals(Constants.TimeType_Year))
            {
                for (int a = 1; a <= 12; a++)
                {
                    totalGoogle = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Google) && i.CreateDate.Month == a).Count();
                    totalFacebook = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Facebook) && i.CreateDate.Month == a).Count();
                    totalEmail = list.Where(i => i.Provider.Equals(Constants.Learner_Provider_Email) && i.CreateDate.Month == a).Count();

                    listLable.Add(a.ToString());
                    listGoogle.Add(totalGoogle);
                    listFacebook.Add(totalFacebook);
                    listEmail.Add(totalEmail);
                }
            }

            return new
            {
                listLable,
                listGoogle,
                listFacebook,
                listEmail
            };
        }

        /// <summary>
        /// Biểu đồ top tỉnh, thành phố
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<object> GetProvince (ReportLearnerSearchConditionModel model)
        {
            DateTime dateFrom = DateTime.Now, dateTo = DateTime.Now;
            int quarterData = model.Quarter;

            if (!model.TimeType.Equals(Constants.TimeType_Between))
            {
                SearchHelper.GetDateFromDateToByTimeType(model.TimeType, model.Year, model.Month, model.Quarter, ref dateFrom, ref dateTo, ref quarterData);
            }
            else
            {
                if (!model.DateFrom.HasValue || !model.DateTo.HasValue)
                {
                    return null;
                }

                dateFrom = model.DateFrom.Value.ToStartDate();
                dateTo = model.DateTo.Value.ToEndDate();
            }
            SearchReportLearnerModel modelSearch = new SearchReportLearnerModel();
            modelSearch.Year = dateFrom.Year;
            modelSearch.TimeType = model.TimeType;
            List<string> listLable = new List<string>();
            List<double> listData = new List<double>();
            var data = await reportLearnerProvinceService.ReportLearnerProvince(modelSearch);

            listLable = data.ReportProvinces.OrderByDescending(i => i.Total).Take(10).Select(i => i.Name).ToList();
            listData = data.ReportProvinces.OrderByDescending(i => i.Total).Take(10).Select(i => i.Total).ToList();

            return new
            {
                listLable,
                listData
            };
        }
    }
}
