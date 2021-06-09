using Elearning.Model.Models.Mobile.MobileLesson;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Model.Models.Mobile.Leson;
using NTS.Common;
using NTS.Common.Resource;
using Elearning.Model.Models.Fontend.Course;
using Elearning.Model.Entities;

namespace Elearning.Mobile.Service.Lesson
{
    public class MobileLessonService : IMobileLessonService
    {
        private readonly ElearningContext _sqlContext;

        public MobileLessonService(ElearningContext _sqlContext)
        {
            this._sqlContext = _sqlContext;
        }

        /// <summary>
        /// Lấy danh sách bài giảng theo id khóa học 
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        public async Task<List<MobileLessonModel>> SearchLessonByCourseId(string id, string userId)
        {

            var lessonHistorys = _sqlContext.LessonHistory.AsNoTracking().Where(a => a.CourseId.Equals(id)).ToList();

            var data = (from a in _sqlContext.LessonCourse.AsNoTracking()
                        join b in _sqlContext.Lesson.AsNoTracking() on a.LessonId equals b.Id
                        where a.CourseId.Equals(id) && b.Status
                        orderby a.DisplayIndex descending
                        select new MobileLessonModel()
                        {
                            Id = b.Id,
                            CourseId = a.CourseId,
                            Name = b.Name,
                            Type = b.Type,
                            Status = false,
                        }).ToList();

            List<MobileLessonFrameModel> list;
            LessonFrameHistory lessonFrameHistory;
            foreach (var item in data)
            {
                list = new List<MobileLessonFrameModel>();
                list = (from a in _sqlContext.LessonFrame.AsNoTracking()
                        where a.LessonId.Equals(item.Id)
                        select new MobileLessonFrameModel
                        {
                            Id = a.Id,
                            Name = a.Name,
                            Type = a.Type
                        }).ToList();
                item.ListLessonFrame = list;

                foreach (var ite in list)
                {
                    lessonFrameHistory = new LessonFrameHistory();
                    lessonFrameHistory = _sqlContext.LessonFrameHistory.FirstOrDefault(i => i.LessonId.Equals(item.Id) && i.LearnerId.Equals(userId) && i.CourseId.Equals(item.CourseId) && i.LessonFrameId.Equals(ite.Id));
                    if (lessonFrameHistory != null)
                    {
                        ite.Status = true;
                    }
                }

                if (item.Type == Constants.Lesson_Type_Study)
                {
                    var isStatus = lessonHistorys.Where(a => a.LessonId.Equals(item.Id) && a.LearnerId.Equals(userId)).FirstOrDefault();
                    if (isStatus != null)
                    {
                        item.Status = true;
                    }
                }
            }

            return data;
        }
        /// <summary>
        /// Chi tiết bài giảng
        /// </summary>
        /// <param name="lessonId"></param>
        /// <returns></returns>
        public async Task<LessonMobileModel> GetLessonByCourseId(string lessonId, string learnerId, string courseId)
        {
            var lesson = (from a in _sqlContext.Lesson.AsNoTracking()
                          where a.Id == lessonId
                          select new LessonMobileModel
                          {
                              LessonId = a.Id,
                              Content = a.Content,
                              ExamTime = a.ExamTime,
                              Name = a.Name,
                              TypeLesson = a.Type,
                              Description = a.Description,
                              ImagePath = a.ImagePath,
                              ListQuestion = (from e in _sqlContext.Question.AsNoTracking()
                                              join f in _sqlContext.LessonQuestion.AsNoTracking()
                                              on e.Id equals f.QuestionId
                                              where f.LessonId == a.Id
                                              select new QuestionMobileModel
                                              {
                                                  Content = e.Content,
                                                  QuestionId = e.Id,
                                                  Type = e.Type,
                                              }).ToList(),
                          }).FirstOrDefault();
            List<AnswerMobileModel> listAnswer;
            if (lesson == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }
            var existLearnerTest = _sqlContext.Test.Where(a => a.LearnerId == learnerId && a.CourseId == courseId && a.LessonId == lessonId).FirstOrDefault();

            string testId = existLearnerTest != null ? existLearnerTest.Id : "";
            lesson.StartDate = existLearnerTest != null ? existLearnerTest.StartDate : (DateTime?)null;
            lesson.FinishDate = existLearnerTest != null ? existLearnerTest.FinishDate : (DateTime?)null;
            lesson.TotalQuestion = existLearnerTest != null ? existLearnerTest.TotalQuestion : 0;
            lesson.TotalCorrect = existLearnerTest != null ? existLearnerTest.TotalCorrect : 0;
            lesson.IsFinish = lesson.StartDate.HasValue && lesson.FinishDate.HasValue;

            if (lesson.TypeLesson == Constants.Lesson_Type_Exam)
            {
                if (!lesson.IsFinish && lesson.StartDate.HasValue) // Kỳ thi chưa kết thúc và có thời gian bắt đầu
                {
                    lesson.RemainingTime = lesson.ExamTime.Value - (DateTime.Now - lesson.StartDate).Value.TotalMinutes;
                }
                else if (!lesson.IsFinish && !lesson.StartDate.HasValue) //TRường hợp kỳ thi chưa kết thúc và chưa có thời gian bắt đầu
                {
                    lesson.RemainingTime = lesson.ExamTime.Value;
                }
            }

            if (lesson.ListQuestion != null)
            {
                foreach (var item in lesson.ListQuestion)
                {

                    listAnswer = new List<AnswerMobileModel>();
                    listAnswer = (from n in _sqlContext.Answer.AsNoTracking()
                                  where n.QuestionId == item.QuestionId
                                  join a in _sqlContext.AnswerLearner.Where(r => r.TestId.Equals(testId)) on n.Id equals a.AnswerId into an
                                  from anv in an.DefaultIfEmpty()
                                  select new AnswerMobileModel
                                  {
                                      AnswerId = n.Id,
                                      AnswerContent = n.AnswerContent,
                                      AnswerLable = n.AnswerLabel,
                                      DisplayIndex = n.DisplayIndex,
                                      IsCorrect = n.IsCorrect,
                                      LearnerIsCorrect = anv != null ? anv.IsCorrect.Value : false,
                                      LearnerAnswerContent = anv != null ? anv.AnswerContent : "",
                                      LearnerDisplayIndex = anv != null ? anv.DisplayIndex.Value : 0,
                                  }).OrderBy(s => s.AnswerLable).ToList();
                    item.ListAnswer = listAnswer;

                    if (lesson.IsFinish)
                    {
                        item.IsResultLearner = CheckResult(item.Type, item.ListAnswer);
                    }
                }
            }

            return lesson;
        }

        /// <summary>
        /// Lấy danh mục con bài giảng chi tiết
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<LessonMobileModel> GetLessonFrameByIdAsync(string id, string learnerId, string courseId)
        {
            var lesson = await (from a in _sqlContext.LessonFrame.AsNoTracking()
                                where a.Id.Equals(id)
                                select new LessonMobileModel
                                {
                                    LessonId = a.LessonId,
                                    Content = a.Content,
                                    ExamTime = a.TestTime,
                                    Name = a.Name,
                                    TypeLesson = a.Type,
                                    ListQuestion = (from e in _sqlContext.Question.AsNoTracking()
                                                    join f in _sqlContext.LessonFrameQuestion.AsNoTracking()
                                                    on e.Id equals f.QuestionId
                                                    where f.LessonFrameId == a.Id
                                                    select new QuestionMobileModel
                                                    {
                                                        Content = e.Content,
                                                        QuestionId = e.Id,
                                                        Type = e.Type,
                                                    }).ToList(),
                                }).FirstOrDefaultAsync();
            List<AnswerMobileModel> listAnswer;
            if (lesson == null)
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            if (lesson.TypeLesson == Constants.Lesson_Type_Exam)
            {
                if (!lesson.IsFinish && lesson.StartDate.HasValue) // Kỳ thi chưa kết thúc và có thời gian bắt đầu
                {
                    lesson.RemainingTime = lesson.ExamTime.Value - (DateTime.Now - lesson.StartDate).Value.TotalMinutes;
                }
                else if (!lesson.IsFinish && !lesson.StartDate.HasValue) //TRường hợp kỳ thi chưa kết thúc và chưa có thời gian bắt đầu
                {
                    lesson.RemainingTime = lesson.ExamTime.Value;
                }
            }

            if (lesson.ListQuestion != null)
            {
                foreach (var item in lesson.ListQuestion)
                {

                    listAnswer = new List<AnswerMobileModel>();
                    listAnswer = (from n in _sqlContext.Answer.AsNoTracking()
                                  where n.QuestionId == item.QuestionId
                                  //join a in _sqlContext.AnswerLearner.Where(r => r.TestId.Equals(testId)) on n.Id equals a.AnswerId into an
                                  //from anv in an.DefaultIfEmpty()
                                  select new AnswerMobileModel
                                  {
                                      AnswerId = n.Id,
                                      AnswerContent = n.AnswerContent,
                                      AnswerLable = n.AnswerLabel,
                                      DisplayIndex = n.DisplayIndex,
                                      IsCorrect = n.IsCorrect,
                                      //LearnerIsCorrect = anv != null ? anv.IsCorrect.Value : false,
                                      //LearnerAnswerContent = anv != null ? anv.AnswerContent : "",
                                      //LearnerDisplayIndex = anv != null ? anv.DisplayIndex.Value : 0,
                                  }).OrderBy(s => s.AnswerLable).ToList();
                    item.ListAnswer = listAnswer;

                    //if (lesson.IsFinish)
                    //{
                    //    item.IsResultLearner = CheckResult(item.Type, item.ListAnswer);
                    //}
                }
            }

            return lesson;
        }

        /// <summary>
        /// Kiểm tra kết quả câu hỏi làm
        /// </summary>
        private bool CheckResult(int type, List<AnswerMobileModel> listAnswer)
        {
            foreach (var answerCompare in listAnswer)
            {
                if (type == 1 || type == 2 || type == 3)
                {
                    if (answerCompare.IsCorrect != answerCompare.LearnerIsCorrect)
                    {
                        return false;
                    }
                }
                else if (type == 4)
                {
                    if (!answerCompare.AnswerContent.Trim().ToLower().Equals(answerCompare.LearnerAnswerContent.Trim().ToLower()))
                    {
                        return false;
                    }
                }
                else if (type == 5)
                {
                    if (answerCompare.DisplayIndex != answerCompare.LearnerDisplayIndex)
                    {
                        return false;
                    }
                }
            }
            return true;
        }

        public async Task CreateLessonHistory(LessonHistoryModel model)
        {
            LessonHistory lessonHistory = new LessonHistory()
            {
                CourseId = model.CourseId,
                FinishDate = model.FinishDate,
                LearnerId = model.LearnerId,
                LessonId = model.LessonId,
                StartDate = DateTime.Now
            };

            _sqlContext.LessonHistory.Add(lessonHistory);
            _sqlContext.SaveChanges();

        }

        /// <summary>
        /// Lưu lịch sử học bài giảng chi tiết
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task CreateLessonFrameHistory(LessonFrameHistorysModel model)
        {
            model.LessonId = _sqlContext.LessonFrame.FirstOrDefault(i => i.Id.Equals(model.LessonFrameId))?.LessonId;

            if (string.IsNullOrEmpty(model.LessonId))
            {
                throw NTSException.CreateInstance(MessageResourceKey.MSG0001, TextResourceKey.Lesson);
            }

            LessonFrameHistory lessonFrameHistory = new LessonFrameHistory()
            {
                LessonFrameId = model.LessonFrameId,
                CourseId = model.CourseId,
                FinishDate = model.FinishDate,
                LearnerId = model.LearnerId,
                LessonId = model.LessonId,
                StartDate = DateTime.Now
            };

            _sqlContext.LessonFrameHistory.Add(lessonFrameHistory);
            await _sqlContext.SaveChangesAsync();
        }
    }
}
