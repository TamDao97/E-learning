using Elearning.Model.Models.Fontend.Exam;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Model.Models.Question;
using Elearning.Model.Models.Answer;
using Elearning.Model.Entities;
using Elearning.Model.Models.Comment;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using Elearning.Services.UserDevice;
using Elearning.Model.Models.UserHistory;
using Elearning.Services.Log;
using Wangkanai.Detection.Services;
using Elearning.Model.Models.LessonFrame;
using Elearning.Model.Models.Lesson;

namespace Elearning.Services.Fontend.Exam
{
    public class ExamService : IExamService
    {
        private readonly IDetectionService _detection;
        private readonly ElearningContext sqlContext;

        public ExamService(ElearningContext sqlContext, IDetectionService _detection)
        {
            this.sqlContext = sqlContext;
            this._detection = _detection;
        }

        /// <summary>
        /// Lấy bài thi
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ExamModel> GetExamByIdAsync(string id, CommentCreateModel model)
        {
            var test = sqlContext.Test.Where(a => a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.CourseId.Equals(model.CourseId)).FirstOrDefault();
            var testId = sqlContext.Test.Where(a => a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.CourseId.Equals(model.CourseId)).FirstOrDefault()?.Id;

            // Lấy thông tin bài giảng
            var result = await (from a in sqlContext.Lesson.AsTracking()
                                join c in sqlContext.LessonCourse.AsNoTracking() on a.Id equals (c.LessonId)
                                where a.Id.Equals(id)
                                select new ExamModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    IsExam = a.IsExam,
                                    ExamTime = a.ExamTime * 60,
                                    Type = a.Type,
                                    TestId = testId,
                                    Slug = a.Slug,
                                    CourseId = c.CourseId,
                                }).FirstOrDefaultAsync();

            if (test != null)
            {
                result.StartDate = test.StartDate;
                result.FinishDate = test.FinishDate;
            }

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (result.FinishDate == null && testId != null)
            {
                TimeSpan ts = DateTime.Now - result.StartDate;
                result.ExamTime = result.ExamTime - Convert.ToInt32(ts.TotalSeconds);
            }

            // Lấy câu hỏi trong bài giảng
            var question = (from a in sqlContext.LessonQuestion.AsNoTracking()
                            join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                            where a.LessonId.Equals(result.Id)
                            orderby b.CreateDate
                            select new QuestionCreateModel
                            {
                                Id = b.Id,
                                Content = b.Content,
                                Type = b.Type,
                                IsLessionQuession = true

                            }).ToList();




            IEnumerable<AnswerInfoModel> listAnswerMode = new List<AnswerInfoModel>();


            if (testId != null)
            {
                foreach (var item in question)
                {
                    listAnswerMode = (from n in sqlContext.Answer.AsNoTracking()
                                      where n.QuestionId == item.Id
                                      join a in sqlContext.AnswerLearner.Where(r => r.TestId.Equals(testId)) on n.Id equals a.AnswerId into an
                                      from anv in an.DefaultIfEmpty()
                                      orderby n.AnswerLabel
                                      select new AnswerInfoModel
                                      {
                                          Id = n.Id,
                                          AnswerLearnerId = anv != null ? anv.AnswerId : null,
                                          AnswerContent = (anv == null && item.Type == 4) ? "" : ((anv == null || anv != null) && item.Type != 4) ? n.AnswerContent : (anv != null && item.Type == 4) ? anv.AnswerContent : "",
                                          AnswerLabel = n.AnswerLabel,
                                          DisplayIndex = anv != null ? anv.DisplayIndex.Value : n.DisplayIndex,
                                          IsCorrect = anv != null ? anv.IsCorrect.Value : false,
                                          Type = item.Type,
                                          AnswerContentQuestion = n.AnswerContent,
                                          DisplayIndexQuestion = n.DisplayIndex,
                                          IsCorrectQuestion = n.IsCorrect,
                                      }).AsQueryable();
                    if (item.Type == 5)
                    {
                        listAnswerMode = listAnswerMode.OrderBy(a => a.DisplayIndex);

                    }
                    item.ListAnswer = listAnswerMode.ToList();
                    item.ListAnswerQuession = listAnswerMode.Where(a => a.IsCorrectQuestion == true).ToList();
                }

            }
            else
            {
                foreach (var item in question)
                {
                    listAnswerMode = (from a in sqlContext.Answer.AsNoTracking()
                                      where a.QuestionId.Equals(item.Id)
                                      orderby a.AnswerLabel
                                      select new AnswerInfoModel
                                      {
                                          Id = a.Id,
                                          QuestionId = a.QuestionId,
                                          AnswerLabel = a.AnswerLabel,
                                          IsCorrect = false,
                                          AnswerContent = item.Type == 4 ? "" : a.AnswerContent,
                                          //DisplayIndex = a.DisplayIndex
                                          Type = item.Type,

                                      }).AsQueryable();
                    item.ListAnswer = listAnswerMode.ToList();

                }
            }

            // Gán list câu hỏi
            result.ListQuestion = question;

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
            else if (result.IsExam)
            {
                result.IsTest = true;
            }

            return result;
        }

        /// <summary>
        /// Lưu tạm đáp án câu hỏi trả lời
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns></returns>
        public async Task CreateTest(SaveTempCreateModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                // trường hợp type là bài tập cuối buổi

                string testId = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault()?.Id;

                if (string.IsNullOrEmpty(testId))
                {
                    Test test = new Test()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CourseId = model.CourseId,
                        LearnerId = model.LearnerId,
                        LessonId = model.LessonId,
                        StartDate = DateTime.Now,
                        FinishDate = null,
                        TotalQuestion = 0,
                        TotalCorrect = 0
                    };
                    sqlContext.Test.Add(test);
                    testId = test.Id;
                }

                var answer = sqlContext.AnswerLearner.Where(a => a.QuestionId.Equals(model.QuestionId) && a.TestId.Equals(testId)).ToList();
                if (answer.Count > 0)
                {
                    sqlContext.RemoveRange(answer);
                }

                AnswerLearner answerLearner;
                List<AnswerLearner> learners = new List<AnswerLearner>();
                foreach (var item1 in model.ListAnswer)
                {
                    answerLearner = new AnswerLearner()
                    {
                        TestId = testId,
                        QuestionId = model.QuestionId,
                        AnswerId = item1.Id,
                        IsCorrect = item1.IsCorrect,
                        AnswerContent = item1.AnswerContent,
                        DisplayIndex = item1.DisplayIndex,
                    };

                    learners.Add(answerLearner);
                }

                sqlContext.AnswerLearner.AddRange(learners);

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// kết thúc bài thi trắc nghiệm
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        public async Task<object> CreateListTest(FinishTestCreateModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                int i = 0;

                string testId = string.Empty;
                //  lấy danh sách câu trả lời câu hỏi
                var listQuestionCompare = (from a in model.ListQuestion
                                           join b in sqlContext.Answer.AsNoTracking() on a.Id equals b.QuestionId into ab
                                           select new QuestionFrontendModel
                                           {
                                               Id = a.Id,
                                               Type = a.Type,
                                               ListAnswer = (from n in ab
                                                             join p in a.ListAnswer on n.Id equals p.Id into np
                                                             from nbv in np.DefaultIfEmpty()
                                                             select new AnserLearnerModel
                                                             {
                                                                 Id = n.Id,
                                                                 AnswerContent = n.AnswerContent,
                                                                 DisplayIndex = n.DisplayIndex,
                                                                 IsCorrect = n.IsCorrect,
                                                                 LearnerAnswerContent = nbv != null ? nbv.AnswerContent : "",
                                                                 LearnerDisplayIndex = nbv != null ? nbv.DisplayIndex : 0,
                                                                 LearnerIsCorrect = nbv != null ? nbv.IsCorrect : false
                                                             }).ToList()
                                           }).ToList();
                //var questions = sqlContext.Question.AsNoTracking().ToList();

                var testInfo = sqlContext.Test.Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault();

                if (testInfo == null)
                {
                    Test test = new Test()
                    {
                        Id = Guid.NewGuid().ToString(),
                        CourseId = model.CourseId,
                        LearnerId = model.LearnerId,
                        LessonId = model.LessonId,
                        StartDate = DateTime.Now,
                        FinishDate = DateTime.Now,
                        TotalQuestion = 0,
                        TotalCorrect = 0
                    };


                    sqlContext.Test.Add(test);
                    testInfo = test;
                }


                var answerLearners = sqlContext.AnswerLearner.Where(a => a.TestId.Equals(testInfo.Id)).ToList();
                if (answerLearners.Count > 0)
                {
                    sqlContext.RemoveRange(answerLearners);
                }
                //Luu dap an nguoi lam
                AnswerLearner answerLearner;
                List<AnswerLearner> learners = new List<AnswerLearner>();
                foreach (var itemQ in model.ListQuestion)
                {
                    foreach (var itemA in itemQ.ListAnswer)
                    {
                        answerLearner = new AnswerLearner()
                        {
                            TestId = testInfo.Id,
                            QuestionId = itemQ.Id,
                            AnswerId = itemA.Id,
                            AnswerContent = itemA.AnswerContent,
                            IsCorrect = itemA.IsCorrect,
                            DisplayIndex = itemA.DisplayIndex
                        };
                        learners.Add(answerLearner);
                    }
                }

                sqlContext.AnswerLearner.AddRange(learners);

                //Tinh diem
                foreach (var itemQuestion in listQuestionCompare)
                {
                    foreach (var answerCompare in itemQuestion.ListAnswer)
                    {
                        if (itemQuestion.Type == 1 || itemQuestion.Type == 2 || itemQuestion.Type == 3)
                        {
                            if (answerCompare.IsCorrect != answerCompare.LearnerIsCorrect)
                            {
                                i++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 4)
                        {
                            if (!answerCompare.AnswerContent.Trim().ToLower().Equals(answerCompare.LearnerAnswerContent.Trim().ToLower()))
                            {
                                i++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 5)
                        {
                            if (answerCompare.DisplayIndex != answerCompare.LearnerDisplayIndex)
                            {
                                i++;
                                break;
                            }
                        }
                    }
                }

                //Luu thoi gian ket thuc
                testInfo.FinishDate = DateTime.Now;
                testInfo.TotalQuestion = model.ListQuestion.Count;
                testInfo.TotalCorrect = model.ListQuestion.Count - i;
                var lessonInfo = sqlContext.Lesson.Where(a => a.Id.Equals(testInfo.LessonId)).First();
                TimeSpan ts = testInfo.FinishDate.Value - testInfo.StartDate;
                var examTime = lessonInfo.ExamTime - Convert.ToInt32(ts.TotalMilliseconds);
                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();

                    return new
                    {
                        totalQuestion = model.ListQuestion.Count,
                        totalRightAnswer = model.ListQuestion.Count - i,
                        totalWrongAnswer = i,
                        finishDate = testInfo.FinishDate,
                        examTime = examTime,
                        //testId = testInfo.Id,
                    };

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }


        }

        /// <summary>
        /// Thêm bài kiểm tra
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<string> CreateTestAsync(HttpRequest request, TestCreateModel model, string userId)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                string testId = null;

                if (model.Type == 2)
                {
                    model.TestId = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault()?.Id;

                }
                if (model.Type == 3)
                {
                    model.TestId = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId)).FirstOrDefault()?.Id;
                }

                if (model.FinishDate != null && !string.IsNullOrEmpty(model.TestId))
                {
                    var test = sqlContext.Test.Where(a => a.Id.Equals(model.TestId)).FirstOrDefault();
                    test.FinishDate = model.FinishDate;
                    test.TotalCorrect = model.TotalCorrect;

                    var answerLearners = sqlContext.AnswerLearner.AsNoTracking().Where(a => a.TestId.Equals(model.TestId)).ToList();
                    if (answerLearners.Count > 0)
                    {
                        sqlContext.RemoveRange(answerLearners);
                    }
                    AnswerLearner answerLearner;
                    List<AnswerLearner> learners = new List<AnswerLearner>();
                    foreach (var item in model.ListQuestion)
                    {

                        foreach (var item1 in item.ListAnswer)
                        {
                            if (!string.IsNullOrEmpty(item1.Checked))
                            {
                                answerLearner = new AnswerLearner()
                                {
                                    TestId = model.TestId,
                                    QuestionId = item.Id,
                                    AnswerId = item1.Id,
                                    AnswerContent = item1.AnswerContent,
                                    IsCorrect = item1.IsCorrect
                                };
                                if (item.Type == 4 || item.Type == 1 || item.Type == 5)
                                {
                                    answerLearner.AnswerContent = item1.Checked;
                                }

                                learners.Add(answerLearner);
                                UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, model.LearnerId);
                                userHistory.Content = "Bạn đã làm xong câu hỏi: " + item.Content;
                                userHistory.Type = 1;
                                LogService.Event(userHistory, _detection);
                            }

                        }


                    }
                    sqlContext.AnswerLearner.AddRange(learners);
                }
                else
                {
                    if (string.IsNullOrEmpty(model.TestId))
                    {
                        Test test = new Test()
                        {
                            Id = Guid.NewGuid().ToString(),
                            CourseId = model.CourseId,
                            LearnerId = model.LearnerId,
                            LessonId = model.LessonId,
                            StartDate = DateTime.Now,
                            FinishDate = model.FinishDate,
                            TotalQuestion = model.TotalQuestion,
                            TotalCorrect = model.TotalCorrect
                        };
                        sqlContext.Test.Add(test);
                        testId = test.Id;
                        AnswerLearner answerLearner;
                        List<AnswerLearner> learners = new List<AnswerLearner>();
                        foreach (var item in model.ListQuestion)
                        {

                            foreach (var item1 in item.ListAnswer)
                            {

                                //var AnswerQuestion = sqlContext.AnswerLearner.AsNoTracking().Where(a => a.AnswerId.Equals(item1.Id)).ToList();
                                //if (AnswerQuestion != null)
                                //{
                                //    sqlContext.RemoveRange(AnswerQuestion);
                                //}

                                if (!string.IsNullOrEmpty(item1.Checked))
                                {
                                    answerLearner = new AnswerLearner()
                                    {
                                        TestId = test.Id,
                                        QuestionId = item.Id,
                                        AnswerId = item1.Id,
                                        AnswerContent = item1.AnswerContent,
                                        IsCorrect = item1.IsCorrect
                                    };
                                    if (item.Type == 4 || item.Type == 1 || item.Type == 5)
                                    {
                                        answerLearner.AnswerContent = item1.Checked;
                                    }
                                    learners.Add(answerLearner);
                                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, model.LearnerId);
                                    userHistory.Content = "Bạn đã làm xong câu hỏi: " + item.Content;
                                    userHistory.Type = 1;

                                    LogService.Event(userHistory, _detection);
                                }

                            }
                        }

                        sqlContext.AnswerLearner.AddRange(learners);
                    }
                    else
                    {
                        AnswerLearner answerLearner;
                        List<AnswerLearner> learners = new List<AnswerLearner>();
                        var question = sqlContext.AnswerLearner.AsNoTracking().Where(a => a.TestId.Equals(model.TestId)).Select(a => a.QuestionId).ToList();
                        var listQuestion = model.ListQuestion.Where(a => !question.Contains(a.Id));
                        foreach (var item in listQuestion)
                        {

                            foreach (var item1 in item.ListAnswer)
                            {
                                if (!string.IsNullOrEmpty(item1.Checked))
                                {
                                    answerLearner = new AnswerLearner()
                                    {
                                        TestId = model.TestId,
                                        QuestionId = item.Id,
                                        AnswerId = item1.Id,
                                        AnswerContent = item1.AnswerContent,
                                        IsCorrect = item1.IsCorrect
                                    };
                                    if (item.Type == 4 || item.Type == 1 || item.Type == 5)
                                    {
                                        answerLearner.AnswerContent = item1.Checked;
                                    }
                                    learners.Add(answerLearner);

                                    UserHistoryModel userHistory = UserDeviceService.GetUserLogHistory(request, model.LearnerId);
                                    userHistory.Content = "Bạn đã làm xong câu hỏi: " + item.Content;
                                    userHistory.Type = 1;

                                    LogService.Event(userHistory, _detection);
                                }

                            }

                        }
                        sqlContext.AnswerLearner.AddRange(learners);
                    }
                }
                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();
                    if (string.IsNullOrEmpty(model.TestId))
                    {
                        return testId;
                    }
                    else
                    {
                        return model.TestId;
                    }
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }
        }

        public async Task<SeachResultModel<ExamModel>> GetListQuestionAnswer(ExamQuestionModel model)
        {

            SeachResultModel<ExamModel> searchResult = new SeachResultModel<ExamModel>();

            // Trường hợp testId là bài giảng luyện tập => cho phép làm lại nhân thêm bản ghi
            if (model.Type == 2)
            {
                var testId = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault()?.Id;

                var listAnswerLearner = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                         join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                         where a.TestId.Equals(testId)
                                         select new ExamModel
                                         {
                                             TestId = a.TestId,
                                             QuestionId = a.QuestionId,
                                             AnswerId = a.AnswerId,
                                             Type = b.Type,
                                             AnswerContent = a.AnswerContent,
                                         }).ToList();

                searchResult.DataResults = listAnswerLearner;

                var examTime = (from a in sqlContext.Test.AsNoTracking()
                                where a.Id.Equals(testId)
                                join b in sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                select new ExamModel
                                {
                                    ExamTime = b.ExamTime,
                                    StartDate = a.StartDate,
                                    Type = b.Type,
                                }).FirstOrDefault();

                var dateNow = DateTime.Now;

                if (examTime != null)
                {
                    TimeSpan Time = dateNow - examTime.StartDate;
                    if (Time.Minutes < examTime.ExamTime)
                    {
                        searchResult.isTime = true;
                        searchResult.TotalTime = examTime.ExamTime - Time.Minutes;

                        var listAnswerLearne = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                                join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                                where a.TestId.Equals(testId)
                                                select new ExamModel
                                                {
                                                    TestId = a.TestId,
                                                    QuestionId = a.QuestionId,
                                                    AnswerId = a.AnswerId,
                                                    Type = b.Type,
                                                    AnswerContent = a.AnswerContent,
                                                }).ToList();

                        searchResult.DataResults = listAnswerLearne;
                    }
                    else
                    {
                        searchResult.isTime = false;
                        searchResult.DataResults = null;
                        var test = sqlContext.Test.Where(a => a.Id.Equals(testId)).FirstOrDefault();
                        test.FinishDate = dateNow;
                        sqlContext.SaveChanges();

                        var listAnswerLearne = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                                join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                                where a.TestId.Equals(null)
                                                select new ExamModel
                                                {
                                                    TestId = a.TestId,
                                                    QuestionId = a.QuestionId,
                                                    AnswerId = a.AnswerId,
                                                    Type = b.Type,
                                                    AnswerContent = a.AnswerContent,
                                                }).ToList();

                        searchResult.DataResults = listAnswerLearne;
                    }
                }
            }

            // Trường hợp testId là bài thi cuối khóa => không cho phép làm lại.

            if (model.Type == 3)
            {
                // trường hợp có finish date
                // Có finish date thì lấy dữ liệu ra và disabal button nộp bài
                var test = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate != null).FirstOrDefault();
                if (test != null)
                {
                    var listAnswerLearner = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                             join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                             where a.TestId.Equals(test.Id)
                                             select new ExamModel
                                             {
                                                 TestId = a.TestId,
                                                 QuestionId = a.QuestionId,
                                                 AnswerId = a.AnswerId,
                                                 Type = b.Type,
                                                 AnswerContent = a.AnswerContent,
                                             }).ToList();

                    searchResult.DataResults = listAnswerLearner;
                    searchResult.isTime = false;
                    searchResult.StateDate = test.StartDate;
                    searchResult.FinishDate = test.FinishDate;
                }
                // trường hợp không có finish date
                // Không có finish date thì lấy dữ liệu ra và kiểm tra xem thời gian hiện tại - thời gian bắt đầu so với examtime
                // Nếu examtime > thì gán giá trị == false dùng để disabal button nộp bài
                // nếu examtime < thì gán giá trị == true dùng hiển thị button nộp bài

                var isTest = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault();

                if (isTest != null)
                {
                    if (test == null)
                    {
                        test = new Test();
                    }

                    var examTime = (from a in sqlContext.Test.AsNoTracking()
                                    where a.Id.Equals(test.Id)
                                    join b in sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                                    select new ExamModel
                                    {
                                        ExamTime = b.ExamTime,
                                        StartDate = a.StartDate,
                                        Type = b.Type,
                                    }).FirstOrDefault();

                    var dateNow = DateTime.Now;

                    if (examTime != null)
                    {
                        TimeSpan Time = dateNow - examTime.StartDate;
                        if (Time.Minutes < examTime.ExamTime)
                        {
                            var listAnswerLearne = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                                    join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                                    where a.TestId.Equals(isTest.Id)
                                                    select new ExamModel
                                                    {
                                                        TestId = a.TestId,
                                                        QuestionId = a.QuestionId,
                                                        AnswerId = a.AnswerId,
                                                        Type = b.Type,
                                                        AnswerContent = a.AnswerContent,
                                                    }).ToList();

                            searchResult.isTime = true;
                            searchResult.TotalTime = examTime.ExamTime - Time.Minutes;
                            searchResult.DataResults = listAnswerLearne;
                            searchResult.StateDate = isTest.StartDate;
                            searchResult.FinishDate = test.FinishDate;


                        }
                        else
                        {
                            var listAnswerLearne = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                                    join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                                    where a.TestId.Equals(null)
                                                    select new ExamModel
                                                    {
                                                        TestId = a.TestId,
                                                        QuestionId = a.QuestionId,
                                                        AnswerId = a.AnswerId,
                                                        Type = b.Type,
                                                        AnswerContent = a.AnswerContent,
                                                    }).ToList();

                            var tesst = sqlContext.Test.Where(a => a.Id.Equals(isTest.Id)).FirstOrDefault();
                            test.FinishDate = dateNow;
                            sqlContext.SaveChanges();
                            searchResult.DataResults = listAnswerLearne;
                            searchResult.StateDate = isTest.StartDate;
                            searchResult.FinishDate = test.FinishDate;

                        }
                    }
                    else
                    {
                        var listAnswerLearner = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                                 join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                                 where a.TestId.Equals(null)
                                                 select new ExamModel
                                                 {
                                                     TestId = a.TestId,
                                                     QuestionId = a.QuestionId,
                                                     AnswerId = a.AnswerId,
                                                     Type = b.Type,
                                                     AnswerContent = a.AnswerContent,
                                                 }).ToList();

                        searchResult.isTime = true;
                        searchResult.DataResults = listAnswerLearner;
                        searchResult.StateDate = isTest.StartDate;
                        searchResult.FinishDate = test.FinishDate;

                    }

                }

                // Trường hợp không tìm thấy bản ghi
                if (test == null && isTest == null)
                {
                    var listAnswerLearner = (from a in sqlContext.AnswerLearner.AsNoTracking()
                                             join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                             where a.TestId.Equals(null)
                                             select new ExamModel
                                             {
                                                 TestId = a.TestId,
                                                 QuestionId = a.QuestionId,
                                                 AnswerId = a.AnswerId,
                                                 Type = b.Type,
                                                 AnswerContent = a.AnswerContent,
                                             }).ToList();

                    searchResult.isTime = true;
                    searchResult.DataResults = listAnswerLearner;
                    searchResult.StateDate = null;
                    searchResult.FinishDate = null;

                }
            }

            return searchResult;
        }

        public async Task<CourseFontendModel> GetLessonCourseAsync(HttpRequest request, string slug, string userId, List<LessonCoursesModel> listCol)
        {
            var result = await (from a in sqlContext.Course.AsNoTracking()
                                where a.Slug.Equals(slug)
                                select new CourseFontendModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    Slug = a.Slug
                                }).FirstOrDefaultAsync();

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
                                               Type = b.Type,
                                               Slug = b.Slug,
                                               Time = b.ExamTime.Value,
                                           }).ToList();

                if (listCol.Count > 0)
                {
                    LessonCoursesModel lessonCourseModel;
                    foreach (var item in result.ListLessonCourse)
                    {
                        lessonCourseModel = new LessonCoursesModel();
                        lessonCourseModel = listCol.FirstOrDefault(i => i.Id.Equals(item.Id));
                        if (lessonCourseModel != null)
                        {
                            item.Col = lessonCourseModel.Col;
                        }
                    }
                }

                LessonHistory lessonHistory;
                LessonFrameHistory lessonFrameHistory;
                List<LessonFrameModel> list;
                int totalHistory = 0;
                int total = 0;
                foreach (var item in result.ListLessonCourse)
                {
                    lessonHistory = new LessonHistory();
                    lessonHistory = sqlContext.LessonHistory.FirstOrDefault(i => i.LessonId.Equals(item.LessonId) && i.LearnerId.Equals(userId) && i.CourseId.Equals(item.CourseId));
                    if (lessonHistory != null)
                    {
                        item.Status = true;
                        item.Percent = 100;
                    }

                    list = new List<LessonFrameModel>();
                    list = (from a in sqlContext.LessonFrame.AsNoTracking()
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

                    total = list.Count();
                    totalHistory = sqlContext.LessonFrameHistory.Where(i => i.LessonId.Equals(item.LessonId) && i.LearnerId.Equals(userId) && i.CourseId.Equals(item.CourseId)).Count();

                    if (total > 0 && totalHistory > 0)
                    {
                        item.Percent = (int)Math.Round((double)totalHistory / total * 100);
                    }

                    foreach (var ite in list)
                    {
                        lessonFrameHistory = new LessonFrameHistory();
                        lessonFrameHistory = sqlContext.LessonFrameHistory.FirstOrDefault(i => i.LessonId.Equals(item.LessonId) && i.LearnerId.Equals(userId) && i.CourseId.Equals(item.CourseId) && i.LessonFrameId.Equals(ite.Id));
                        if (lessonFrameHistory != null)
                        {
                            ite.Status = true;
                        }

                        if (ite.Type == Constants.Lesson_Type_Study)
                        {
                            var question = (from a in sqlContext.LessonFrameQuestion.AsNoTracking()
                                            join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                                            join c in sqlContext.Topic.AsNoTracking() on b.TopicId equals c.Id
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
                            ite.ListQuestion = question;
                        }
                    }

                    item.ListLessonFrame = list;
                }
            }
            return result;
        }

        public async Task<string> GetLessonIdByslug(string slug)
        {
            var lesson = sqlContext.Lesson.FirstOrDefault(s => s.Slug == slug);
            if (lesson != null)
            {
                return lesson.Id;
            }
            return null;
        }

        /// <summary>
        /// Lấy bài thi trắc nghiệm danh mục
        /// </summary>
        /// <param name="slug"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<ExamModel> GetExamFrameByIdAsync(string id, CommentCreateModel model)
        {
            var test = sqlContext.LessonFrameHistory.Where(a => a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.CourseId.Equals(model.CourseId) && a.LessonFrameId.Equals(id)).FirstOrDefault();
            var testId = sqlContext.Test.Where(a => a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.CourseId.Equals(model.CourseId)).FirstOrDefault()?.Id;

            // Lấy thông tin bài giảng
            var result = await (from a in sqlContext.LessonFrame.AsTracking()
                                join c in sqlContext.Lesson.AsNoTracking() on a.LessonId equals c.Id
                                where a.Id.Equals(id)
                                select new ExamModel
                                {
                                    Id = a.Id,
                                    Name = a.Name,
                                    //IsExam = a.IsExam,
                                    ExamTime = a.TestTime * 60,
                                    Type = a.Type,
                                    TestId = testId,
                                    Slug = c.Slug,
                                    //CourseId = c.CourseId,
                                    TotalQuestion = a.TotalQuestion,
                                    TotalCorrect = a.TotalRequestCorrect
                                }).FirstOrDefaultAsync();

            if (test != null)
            {
                result.StartDate = test.StartDate.HasValue ? test.StartDate.Value : DateTime.Now;
                result.FinishDate = test.FinishDate;
            }

            if (result == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (result.FinishDate == null && testId != null)
            {
                TimeSpan ts = DateTime.Now - result.StartDate;
                result.ExamTime = result.ExamTime - Convert.ToInt32(ts.TotalSeconds);
            }

            // Lấy câu hỏi trong bài giảng
            var question = (from a in sqlContext.LessonFrameQuestion.AsNoTracking()
                            join b in sqlContext.Question.AsNoTracking() on a.QuestionId equals b.Id
                            where a.LessonFrameId.Equals(result.Id)
                            orderby b.CreateDate
                            select new QuestionCreateModel
                            {
                                Id = b.Id,
                                Content = b.Content,
                                Type = b.Type,
                                IsLessionQuession = true
                            }).ToList();

            IEnumerable<AnswerInfoModel> listAnswerMode = new List<AnswerInfoModel>();

            if (testId != null)
            {
                foreach (var item in question)
                {
                    listAnswerMode = (from n in sqlContext.Answer.AsNoTracking()
                                      where n.QuestionId == item.Id
                                      join a in sqlContext.AnswerLearner.Where(r => r.TestId.Equals(testId)) on n.Id equals a.AnswerId into an
                                      from anv in an.DefaultIfEmpty()
                                      orderby n.AnswerLabel
                                      select new AnswerInfoModel
                                      {
                                          Id = n.Id,
                                          AnswerLearnerId = anv != null ? anv.AnswerId : null,
                                          AnswerContent = (anv == null && item.Type == 4) ? "" : ((anv == null || anv != null) && item.Type != 4) ? n.AnswerContent : (anv != null && item.Type == 4) ? anv.AnswerContent : "",
                                          AnswerLabel = n.AnswerLabel,
                                          DisplayIndex = anv != null ? anv.DisplayIndex.Value : n.DisplayIndex,
                                          IsCorrect = anv != null ? anv.IsCorrect.Value : false,
                                          Type = item.Type,
                                          AnswerContentQuestion = n.AnswerContent,
                                          DisplayIndexQuestion = n.DisplayIndex,
                                          IsCorrectQuestion = n.IsCorrect,
                                      }).AsQueryable();
                    if (item.Type == 5)
                    {
                        listAnswerMode = listAnswerMode.OrderBy(a => a.DisplayIndex);

                    }
                    item.ListAnswer = listAnswerMode.ToList();
                    item.ListAnswerQuession = listAnswerMode.Where(a => a.IsCorrectQuestion == true).ToList();
                }

            }
            else
            {
                foreach (var item in question)
                {
                    listAnswerMode = (from a in sqlContext.Answer.AsNoTracking()
                                      where a.QuestionId.Equals(item.Id)
                                      orderby a.AnswerLabel
                                      select new AnswerInfoModel
                                      {
                                          Id = a.Id,
                                          QuestionId = a.QuestionId,
                                          AnswerLabel = a.AnswerLabel,
                                          IsCorrect = false,
                                          AnswerContent = item.Type == 4 ? "" : a.AnswerContent,
                                          //DisplayIndex = a.DisplayIndex
                                          Type = item.Type,

                                      }).AsQueryable();
                    item.ListAnswer = listAnswerMode.ToList();
                }
            }

            // Gán list câu hỏi
            result.ListQuestion = question;

            var check = sqlContext.LessonFrameHistory.FirstOrDefault(i => i.LessonId.Equals(model.LessonId) && i.LearnerId.Equals(model.ObjectId) && i.CourseId.Equals(model.CourseId) && i.LessonFrameId.Equals(id));
            if (check == null)
            {
                LessonFrameHistory lessonFrameHistory = new LessonFrameHistory()
                {
                    //Id = Guid.NewGuid().ToString(),
                    LessonFrameId = result.Id,
                    LessonId = model.LessonId,
                    LearnerId = model.ObjectId,
                    CourseId = model.CourseId,
                    StartDate = DateTime.Now,
                    TotalQuestion = result.TotalQuestion,
                    TotalCorrect = result.TotalCorrect,
                    Pass = false,
                };
                sqlContext.LessonFrameHistory.Add(lessonFrameHistory);
                sqlContext.SaveChanges();
            }
            else if (result.IsExam)
            {
                result.IsTest = true;
            }

            return result;
        }

        /// <summary>
        /// Kết thúc bài thi trắc nghiệm danh mục
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        public async Task<object> CreateListLessonFrame(string id, FinishTestCreateModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                int i = 0;

                string testId = string.Empty;
                //  lấy danh sách câu trả lời câu hỏi
                var listQuestionCompare = (from a in model.ListQuestion
                                           join b in sqlContext.Answer.AsNoTracking() on a.Id equals b.QuestionId into ab
                                           select new QuestionFrontendModel
                                           {
                                               Id = a.Id,
                                               Type = a.Type,
                                               ListAnswer = (from n in ab
                                                             join p in a.ListAnswer on n.Id equals p.Id into np
                                                             from nbv in np.DefaultIfEmpty()
                                                             select new AnserLearnerModel
                                                             {
                                                                 Id = n.Id,
                                                                 AnswerContent = n.AnswerContent,
                                                                 DisplayIndex = n.DisplayIndex,
                                                                 IsCorrect = n.IsCorrect,
                                                                 LearnerAnswerContent = nbv != null ? nbv.AnswerContent : "",
                                                                 LearnerDisplayIndex = nbv != null ? nbv.DisplayIndex : 0,
                                                                 LearnerIsCorrect = nbv != null ? nbv.IsCorrect : false
                                                             }).ToList()
                                           }).ToList();
                //var questions = sqlContext.Question.AsNoTracking().ToList();

                var testInfo = sqlContext.LessonFrameHistory.Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.LessonFrameId.Equals(id)).FirstOrDefault();

                var answerLearners = sqlContext.LessonAnswerLearner.Where(a => a.LessonFrameHistoryId.Equals(testInfo.Id)).ToList();
                if (answerLearners.Count > 0)
                {
                    sqlContext.LessonAnswerLearner.RemoveRange(answerLearners);
                }
                //Luu dap an nguoi lam
                LessonAnswerLearner answerLearner;
                List<LessonAnswerLearner> learners = new List<LessonAnswerLearner>();
                foreach (var itemQ in model.ListQuestion)
                {
                    foreach (var itemA in itemQ.ListAnswer)
                    {
                        answerLearner = new LessonAnswerLearner()
                        {
                            LessonFrameHistoryId = testInfo.Id,
                            QuestionId = itemQ.Id,
                            AnswerId = itemA.Id,
                            AnswerContent = itemA.AnswerContent,
                            IsCorrect = itemA.IsCorrect,
                            DisplayIndex = itemA.DisplayIndex
                        };
                        learners.Add(answerLearner);
                    }
                }

                sqlContext.LessonAnswerLearner.AddRange(learners);

                //Tinh diem
                foreach (var itemQuestion in listQuestionCompare)
                {
                    foreach (var answerCompare in itemQuestion.ListAnswer)
                    {
                        if (itemQuestion.Type == 1 || itemQuestion.Type == 2 || itemQuestion.Type == 3)
                        {
                            if (answerCompare.IsCorrect != answerCompare.LearnerIsCorrect)
                            {
                                i++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 4)
                        {
                            if (!answerCompare.AnswerContent.Trim().ToLower().Equals(answerCompare.LearnerAnswerContent.Trim().ToLower()))
                            {
                                i++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 5)
                        {
                            if (answerCompare.DisplayIndex != answerCompare.LearnerDisplayIndex)
                            {
                                i++;
                                break;
                            }
                        }
                    }
                }

                //Luu thoi gian ket thuc
                testInfo.FinishDate = DateTime.Now;
                //testInfo.TotalQuestion = model.ListQuestion.Count;
                //testInfo.TotalCorrect = testInfo.TotalCorrect;
                if (i >= testInfo.TotalCorrect)
                {
                    testInfo.Pass = true;
                }
                var lessonInfo = sqlContext.Lesson.Where(a => a.Id.Equals(testInfo.LessonId)).First();
                TimeSpan ts = testInfo.FinishDate.Value - testInfo.StartDate.Value;
                var examTime = lessonInfo.ExamTime - Convert.ToInt32(ts.TotalMilliseconds);
                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();

                    return new
                    {
                        totalQuestion = model.ListQuestion.Count,
                        totalRightAnswer = model.ListQuestion.Count - i,
                        totalWrongAnswer = i,
                        finishDate = testInfo.FinishDate,
                        examTime = examTime,
                        //testId = testInfo.Id,
                    };

                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    throw ex;
                }
            }


        }
    }
}
