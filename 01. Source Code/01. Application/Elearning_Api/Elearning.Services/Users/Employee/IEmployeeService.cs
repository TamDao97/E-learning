using Elearning.Model.Models.User.Employee;
using Elearning.Model.Models.User.Learner;
using Elearning.Models.Base;
using Elearning.Models.GroupUser;
using Elearning.Models.User.Employee;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Elearning.Services.Users.Employee
{
    public interface IEmployeeService
    {

        /// <summary>
        /// Tìm kiếm nhân viên
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        Task<SearchBaseResultModel<EmployeeResultModel>> SearchEmployeeAsync (EmployeeSearchConditionModel searchModel);
        /// <summary>
        /// Lấy thông tin nhân viên theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<EmployeeInfoModel> GetEmployeeByIdAsync (string id, string userId);
        /// <summary>
        /// Thêm nhân viên
        /// </summary>
        /// <param name="employeeModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task CreateEmployeeAsync (EmployeeCreateModel employeeModel, string userId);
        /// <summary>
        /// Cập nhập nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="employeeModel"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task UpdateEmployeeAsync (string id, EmployeeCreateModel employeeModel, string userId);
        /// <summary>
        /// Xóa nhân viên
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task DeleteEmployeeByIdAsync (string id, string userId);
        /// <summary>
        /// Lấy danh danh sách chức năng
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        List<GroupFunctionModel> GetPermisstions (string userId);
        /// <summary>
        /// Danh sách nhóm chức năng
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        List<PermissionModel> GetGroupPermission (string groupUserId);

        /// <summary>
        /// Tìm kiếm hướng dẫn viên
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<EmployeeResultModel>> SearchMentorAsync(EmployeeSearchConditionModel  searchModel);

        /// <summary>
        /// Tìm kiếm người học
        /// </summary>
        /// <returns></returns>
        Task<SearchBaseResultModel<LearnerResultModel>> SearchLearnerAsync(EmployeeSearchConditionModel searchModel);
    }
}
