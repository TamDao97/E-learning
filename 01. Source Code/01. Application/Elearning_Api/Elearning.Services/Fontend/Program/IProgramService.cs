using Elearning.Model.Models.Fontend.Client_Program;
using Elearning.Model.Models.Fontend.Course;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.Program
{
    public interface IProgramService
    {
        /// <summary>
        ///  Lấy danh sách chương trình đào tạo 
        /// </summary>
        /// <returns></returns>
        Task<List<ProgramModel>> GetListProgram(string id);
        /// <summary>
        /// Load ra danh sách chương trình và khóa học của chương trình đó
        /// </summary>
        /// <returns></returns>
        Task<List<ProgramModel>> GetProgramAsync(string learnerId);

        /// <summary>
        /// Lấy chi tiết chương trình đào tạo
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ProgramModel> GetProgramByIdAsync(SearchProgram searchProgram);
        Task<CourseModel> GetTop2Course ();
        Task CreateLearnerCourseAsync (HttpRequest request, LearnerCourseCreateModel model);

    }
}
