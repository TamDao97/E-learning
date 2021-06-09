using Elearning.Model.Models.Fontend.Client_Program;
using Elearning.Model.Models.Fontend.Course;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.Course
{
    public interface ICourseService
    {
        /// <summary>
        /// Lấy chi tiết khóa học
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<CourseDetailModel> GetCourseById(CourseIdModel courseIdModel);
        Task<List<CourseSearchFrontendModel>> SearchCourseAsync (string learnerId, string searchValue);
    }
}
