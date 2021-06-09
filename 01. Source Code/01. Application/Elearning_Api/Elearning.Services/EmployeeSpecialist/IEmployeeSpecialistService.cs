using Elearning.Model.Models.EmployeeSpecialist;
using Elearning.Models.Base;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.EmployeeSpecialist
{
    public interface IEmployeeSpecialistService
    {
        Task<HomeSpecialistResultModel> SearchEmployeeSpecialistAsync();

        Task CreateEmployeeSpecialistAsync(HttpRequest request, EmployeeSpecialistCreateModel model, string userId);

        Task UpdateEmployeeSpecialistAsync(HttpRequest request, int id, EmployeeSpecialistCreateModel model, string userId);

        Task<EmployeeSpecialistModel> GetEmployeeSpecialistByIdAsync(int id);

        Task DeleteEmployeeSpecialistByIdAsync(HttpRequest request, int id, string userId);

        Task<SearchBaseResultModel<EmployeeSpecialResultModel>> SearchEmployee(EmployeeSearchModel searchModel);
    }
}
