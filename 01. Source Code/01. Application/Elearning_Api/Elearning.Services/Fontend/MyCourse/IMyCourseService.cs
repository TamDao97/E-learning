using Elearning.Model.Models.Fontend.Client_Program;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.MyCourse
{
    public interface IMyCourseService
    {
        Task<List<CourseModel>> GetMyCourse (string learnerId);
    }
}
