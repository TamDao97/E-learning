using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.Combobox;

namespace Elearning.Services.Combobox
{
    public interface IComboboxService
    {
        /// <summary>
        /// Lấy danh sách loại barcode
        /// </summary>
        /// <returns></returns>
        Task<ApiResultModel> GetAllBarcodeType();

        /// <summary>
        /// Lấy danh sách loại mã
        /// </summary>
        /// <returns></returns>
        Task<ApiResultModel> GetAllCodeType();

        /// <summary>
        /// Lấy danh sách loại bao bì
        /// </summary>
        /// <returns></returns>
        Task<ApiResultModel> GetAllParkingType();

        /// <summary>
        /// Danh sách nhóm tài khoản
        /// </summary>
        /// <returns></returns>
        List<ComboboxModel> GetListGroupuser();

        /// <summary>
        /// Danh sách chủ đề
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxParentModel>> GetCategoryAsync();

        /// <summary>
        /// Danh sách danh mục
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxTopicFullModel>> GetCategoryFullAsync();

        /// <summary>
        /// Lấy danh sách chương trình
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxModel>> GetAllProgram();

        /// <summary>
        /// Danh sách chủ đề câu hỏi load theo parent
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxTopicFullModel>> GetTopicFullAsync();

        /// <summary>
        /// Danh sách chủ đề
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxParentModel>> GetTopicAsync();

        Task<List<CbbOrderModel>> SearchHomeSlider();

        Task<List<ComboboxModel>> GetEmployeeAsync();

        Task<List<ComboboxIntModel>> GetHomeSpecialistAsync();

        Task<SearchBaseResultModel<ComboboxModel>> GetListProvince();
        Task<SearchBaseResultModel<ComboboxIntModel>> GetListNation();
        Task<SearchBaseResultModel<ComboboxModel>> GetListDistrictByProvinceId(string ProvinceId);
        Task<SearchBaseResultModel<ComboboxModel>> GetListWardByDistrictId(string DistrictId);

        Task<SearchBaseResultModel<ComboboxModel>> GetUser();

        Task<SearchBaseResultModel<ComboboxModel>> GetLearner();

        /// <summary>
        /// Lấy danh sách đơn vị chủ quản
        /// </summary>
        /// <returns></returns>
        Task<List<ComboboxUnitModel>> GetListManageUnits();
    }
}
