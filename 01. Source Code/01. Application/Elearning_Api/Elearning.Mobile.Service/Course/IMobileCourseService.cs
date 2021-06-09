using Elearning.Model.MobileCourse;
using Elearning.Model.Models.Mobile.Course;
using Elearning.Model.Models.Mobile.Program;
using Elearning.Model.Models.MobileCourse;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Mobile.Course
{
    public interface IMobileCourseService
    {
        /// <summary>
        /// Lấy danh sách khóa học theo id chương trình đào tạo
        /// </summary>
        /// <param name="id">id chương trình đào tạo</param>
        /// <returns></returns>
        Task<List<MobileCourseResultModel>> SearchCourseByIdProgram(string id, string learnerId);

        /// <summary>
        /// Lấy chi tiết khóa học theo id
        /// </summary>
        /// <param name="id">id khóa học</param>
        /// <returns></returns>
        Task<MobileCourseDetailModel> GetInfoCourseById(string id, string learnerId);

        /// <summary>
        /// Đăng ký khóa học
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task RegisterCourseAsync (MobileLearnerCourseCreateModel model);

        /// <summary>
        /// Tìm kếm khóa học theo tên hoặc mô tả
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<List<MobileCourseSearchResultModel>> SearchCourse (MobileCourseSearchModel model);

        /// <summary>
        /// Lấy danh sách khóa học của tôi
        /// </summary>
        /// <param name="learnerId"></param>
        /// <returns></returns>
        Task<MobileMyCourseModel> MyCourse (string learnerId);

        /// <summary>
        /// Danh sách hướng dẫn viên theo khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<List<MobileEmployeeCourseModel>> GetListEmployeeCourse(string id);
    }
}
