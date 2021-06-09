using Elearning.Model.Models.Fontend.Expert;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Fontend.Expert
{
    public interface IExpertService
    {
        Task<List<ExpertModel>> GetExpertAsync ();
    }
}
