using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Model.Models.Mobile.Program;

namespace Elearning.Mobile.Service.Program
{
    public interface IMobileProgramService
    {
        /// <summary>
        /// Danh sách chương trình học
        /// </summary>
        /// <returns></returns>
        Task<List<MobileProgramModel>> GetListProgram (string learnerId);
        /// <summary>
        /// Chi tiết chương trình
        /// </summary>
        /// <param name="id"></param>
        /// <param name="learnerId"></param>
        /// <returns></returns>
        Task<MobileProgramDetailModel> GetProgramDetailById (string id, string learnerId);

    }
}
