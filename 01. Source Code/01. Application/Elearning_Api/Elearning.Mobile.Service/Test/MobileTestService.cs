using Elearning.Model.Entities;
using Elearning.Model.Models.Mobile.MobileAnswer;
using Elearning.Model.Models.Mobile.MobileQuestion;
using Elearning.Model.Models.Mobile.MobileTest;
using Elearning.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.Test
{
    public class MobileTestService : IMobileTestService
    {
        private readonly ElearningContext sqlContext;
        public MobileTestService(ElearningContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        /// <summary>
        /// Lưu tạm đáp án câu hỏi trả lời
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns></returns>
        public async Task CreateTest(MobileTestCreateModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                // trường hợp type là bài tập cuối buổi

                string testId = "";
                var testInfo = sqlContext.Test.AsNoTracking().Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId)).FirstOrDefault();
                AnswerLearner answerLearner;
                List<AnswerLearner> learners = new List<AnswerLearner>();
                if (testInfo == null)
                {
                    Elearning.Model.Entities.Test test = new Elearning.Model.Entities.Test()
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
                else
                {
                    //Kỳ thi đã kết thúc không lưu đáp án nữa
                    if (testInfo.StartDate != null && testInfo.FinishDate.HasValue)
                    {
                        return;
                    }

                    testId = testInfo.Id;
                }

                var answerLearners = sqlContext.AnswerLearner.Where(a => a.QuestionId.Equals(model.QuestionId) && a.TestId.Equals(testId)).ToList();
                if (answerLearners.Count > 0)
                {
                    sqlContext.RemoveRange(answerLearners);
                }


                foreach (var item1 in model.ListAnswer)
                {

                    answerLearner = new AnswerLearner()
                    {
                        TestId = testId,
                        QuestionId = model.QuestionId,
                        AnswerId = item1.AnswerId,
                        IsCorrect = item1.LearnerIsCorrect,
                        AnswerContent = item1.LearnerAnswerContent,
                        DisplayIndex = item1.LearnerDisplayIndex,
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
        public async Task<object> CreateListTest(MobileTestCreateListModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                int totalWrong = 0;

                string testId = string.Empty;
                //  lấy danh sách câu trả lời câu hỏi
                var listQuestionCompare = (from a in model.ListQuestion
                                           join b in sqlContext.Answer.AsNoTracking() on a.QuestionId equals b.QuestionId into ab
                                           select new MobileQuestionModel
                                           {
                                               QuestionId = a.QuestionId,
                                               Type = a.Type,
                                               ListAnswer = (from n in ab
                                                             join p in a.ListAnswer on n.Id equals p.AnswerId into np
                                                             from nbv in np.DefaultIfEmpty()
                                                             select new MobileAnswerLearnerModel
                                                             {
                                                                 AnswerId = n.Id,
                                                                 AnswerContent = n.AnswerContent,
                                                                 DisplayIndex = n.DisplayIndex,
                                                                 IsCorrect = n.IsCorrect,
                                                                 LearnerAnswerContent = nbv != null ? nbv.LearnerAnswerContent : "",
                                                                 LearnerDisplayIndex = nbv != null ? nbv.LearnerDisplayIndex : 0,
                                                                 LearnerIsCorrect = nbv != null ? nbv.LearnerIsCorrect : false
                                                             }).ToList()
                                           }).ToList();

                var testInfo = sqlContext.Test.Where(a => a.CourseId.Equals(model.CourseId) && a.LearnerId.Equals(model.LearnerId) && a.LessonId.Equals(model.LessonId) && a.FinishDate == null).FirstOrDefault();


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
                            QuestionId = itemQ.QuestionId,
                            AnswerId = itemA.AnswerId,
                            AnswerContent = itemA.LearnerAnswerContent,
                            IsCorrect = itemA.LearnerIsCorrect,
                            DisplayIndex = itemA.LearnerDisplayIndex
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
                                totalWrong++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 4)
                        {
                            if (!answerCompare.AnswerContent.Trim().ToLower().Equals(answerCompare.LearnerAnswerContent.Trim().ToLower()))
                            {
                                totalWrong++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 5)
                        {
                            if (answerCompare.DisplayIndex != answerCompare.LearnerDisplayIndex)
                            {
                                totalWrong++;
                                break;
                            }
                        }
                    }
                }

                //Luu thoi gian ket thuc
                testInfo.FinishDate = DateTime.Now;
                testInfo.TotalQuestion = model.ListQuestion.Count;
                testInfo.TotalCorrect = model.ListQuestion.Count - totalWrong;

                try
                {
                    await sqlContext.SaveChangesAsync();
                    trans.Commit();

                    return new
                    {
                        totalQuestion = testInfo.TotalQuestion,
                        totalRightAnswer = testInfo.TotalCorrect,
                        totalWrongAnswer = totalWrong,
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
        /// kết thúc bài thi trắc nghiệm bài giảng chi tiết
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <returns>Trả lại object câu trả lời đúng, câu trả lời sai, tổng số câu hỏi</returns>
        public async Task<object> CreateListTestLessonFrame(string id, MobileTestCreateListModel model)
        {
            using (var trans = sqlContext.Database.BeginTransaction())
            {
                int totalWrong = 0;

                string testId = string.Empty;
                //  lấy danh sách câu trả lời câu hỏi
                List<MobileQuestionModel> listQuestionCompare = new List<MobileQuestionModel>();
                //listQuestionCompare = (from a in model.ListQuestion
                //                       join b in sqlContext.Answer.AsNoTracking() on a.QuestionId equals b.QuestionId into ab
                //                       select new MobileQuestionModel
                //                       {
                //                           QuestionId = a.QuestionId,
                //                           Type = a.Type,
                //                           ListAnswer = (from n in ab
                //                                         join p in a.ListAnswer on n.Id equals p.AnswerId into np
                //                                         from nbv in np.DefaultIfEmpty()
                //                                         select new MobileAnswerLearnerModel
                //                                         {
                //                                             AnswerId = n.Id,
                //                                             AnswerContent = n.AnswerContent,
                //                                             DisplayIndex = n.DisplayIndex,
                //                                             IsCorrect = n.IsCorrect,
                //                                             LearnerAnswerContent = nbv != null ? nbv.LearnerAnswerContent : "",
                //                                             LearnerDisplayIndex = nbv != null ? nbv.LearnerDisplayIndex : 0,
                //                                             LearnerIsCorrect = nbv != null ? nbv.LearnerIsCorrect : false
                //                                         }).ToList()
                //                       }).ToList();

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
                            QuestionId = itemQ.QuestionId,
                            AnswerId = itemA.AnswerId,
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
                                totalWrong++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 4)
                        {
                            if (!answerCompare.AnswerContent.Trim().ToLower().Equals(answerCompare.LearnerAnswerContent.Trim().ToLower()))
                            {
                                totalWrong++;
                                break;
                            }
                        }
                        else if (itemQuestion.Type == 5)
                        {
                            if (answerCompare.DisplayIndex != answerCompare.LearnerDisplayIndex)
                            {
                                totalWrong++;
                                break;
                            }
                        }
                    }
                }

                //Luu thoi gian ket thuc
                testInfo.FinishDate = DateTime.Now;
                //testInfo.TotalQuestion = model.ListQuestion.Count;
                //testInfo.TotalCorrect = testInfo.TotalCorrect;
                if (totalWrong >= testInfo.TotalCorrect)
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
                        totalRightAnswer = model.ListQuestion.Count - totalWrong,
                        totalWrongAnswer = totalWrong,
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
