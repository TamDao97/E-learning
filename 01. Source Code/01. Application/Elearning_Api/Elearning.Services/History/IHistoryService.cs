using Elearning.Model.Models.History;
using Elearning.Models.Base;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.History
{
    public interface IHistoryService
    {
        Task<SearchBaseResultModel<HistoryModel>> SearchHistory(HistorySearchModel modelSearch);
    }
}
