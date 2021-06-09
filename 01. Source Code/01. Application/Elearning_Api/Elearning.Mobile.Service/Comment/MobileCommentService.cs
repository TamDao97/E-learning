using Elearning.Model.Models.Mobile.MobileComment;
using Elearning.Model.Models.MobileComment;
using Elearning.Models.Base;
using Elearning.Models.Entities;
using Elearning.Models.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using NTS.Common;
using NTS.Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Mobile.Service.Comment
{
    public class MobileCommentService : IMobileCommentService
    {
        private readonly ElearningContext _sqlContext;
        private readonly AppSettingModel appSettingModel;

        public MobileCommentService(ElearningContext _sqlContext, IOptions<AppSettingModel> options)
        {
            this._sqlContext = _sqlContext;
            this.appSettingModel = options.Value;

        }

        /// <summary>
        /// Thêm mới thông tin phản hồi theo bài giảng
        /// </summary>
        /// <param name="model">Dữ liệu thông tin phản hồi</param>
        /// <returns></returns>
        public async Task CreateComment(MobileCommentModel model)
        {
            var parentComment = _sqlContext.Comment.FirstOrDefault(i => i.Id == model.ParentCommentId);
            if (parentComment != null)
            {
                parentComment.Status = Constants.Comment_Status_Approved;
            }

            Elearning.Model.Entities.Comment comment = new Model.Entities.Comment()
            {
                CourseId = model.CourseId,
                LessonId = model.LessonId,
                ObjectType = Constants.User_UserType_Student,
                ParentCommentId = model.ParentCommentId,
                Status = Constants.Comment_Status_Pending,
                CommentDate = DateTime.Now,
                Type = model.Type,
                Content = model.Content,
                ObjectId = model.LearnerId
            };

            _sqlContext.Comment.Add(comment);
            await _sqlContext.SaveChangesAsync();
        }

        /// <summary>
        /// Danh sách phản hồi bài giảng
        /// </summary>
        /// <param name="searchModel">Dữ liệu tìm kiếm</param>
        /// <returns></returns>
        public async Task<SearchBaseResultModel<MobileCommentResultModel>> SearchComment(MobileCommentSearchModel searchModel)
        {
            SearchBaseResultModel<MobileCommentResultModel> searchResult = new SearchBaseResultModel<MobileCommentResultModel>();

            var data = await (from a in _sqlContext.Comment.AsNoTracking()
                              join b in _sqlContext.User.AsNoTracking() on a.ObjectId equals b.Id into ab
                              from abv in ab.DefaultIfEmpty()
                              join d in _sqlContext.Employee on abv.ObjectId equals d.Id into bd
                              from bdv in bd.DefaultIfEmpty()
                              join c in _sqlContext.Learner.AsNoTracking() on a.ObjectId equals c.Id into ac
                              from acv in ac.DefaultIfEmpty()
                              where (a.CourseId.Equals(searchModel.CourseId) || a.LessonId.Equals(searchModel.LessonId)) && a.Type == searchModel.Type
                              && ((a.Status == Constants.Comment_Status_Approved && !a.ObjectId.Equals(searchModel.UserId))
                              || a.ObjectId.Equals(searchModel.UserId))
                              orderby a.CommentDate descending
                              select new MobileCommentResultModel
                              {
                                  Id = a.Id,
                                  ParentCommentId = a.ParentCommentId,
                                  Content = a.Content,
                                  CommentDate = a.CommentDate,
                                  Status = a.Status,
                                  ImagePath = bdv != null ? !string.IsNullOrEmpty(bdv.Avatar) ? appSettingModel.ServerApiUrl + bdv.Avatar : "" : !string.IsNullOrEmpty(acv.Avatar) ? appSettingModel.ServerApiUrl + acv.Avatar : "",
                                  UserName = bdv != null ? bdv.Name : acv.Name
                              }).ToListAsync();

            List<MobileCommentResultModel> listChild = new List<MobileCommentResultModel>();
            var listParent = data.Where(r => !r.ParentCommentId.HasValue).ToList();
            var listChildren = data.Where(r => r.ParentCommentId.HasValue).ToList();
            foreach (var parent in listParent)
            {

                parent.ListReply = GetCommentChild(parent.Id, listChildren);
                listChild.Add(parent);
            }

            searchResult.DataResults = listChild;
            return searchResult;
        }

        private List<MobileCommentResultModel> GetCommentChild(long parentId, List<MobileCommentResultModel> listComment)
        {
            List<MobileCommentResultModel> listChild = new List<MobileCommentResultModel>();
            // Lấy danh sách bản ghi có parentId
            var listChildSub = listComment.Where(r => parentId.Equals(r.ParentCommentId)).ToList();

            foreach (var child in listChildSub)
            {
                child.ListReply = GetCommentChild(child.Id, listComment);
                listChild.Add(child);
            }

            return listChild;
        }
    }
}
