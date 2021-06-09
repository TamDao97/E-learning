using Elearning.Model.Models.CompleteStatistics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.CompleteStatistics
{
    public interface ICompleteStatisticsService
    {
        Task<List<ResultCompleteModel>>StatisticalCompleteCourse (SearchCompleteModel model);
        Task<string> Export(SearchCompleteModel model, int type);
    }
}
