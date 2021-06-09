using Elearning.Model.Models.NumberSubscribersStatistics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.NumberOfSubscribersStatistics
{
   public interface INumberOfSubscribersService
    {
        Task<List<ResultNumberSubscribersModel>> StatisticalNumberSubscribers (SearchNumberSubscribersModel model);
        Task<string> Export (SearchNumberSubscribersModel model, int type);
    }
}
