using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Elearning.Api.Attributes;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.UserAdmin;
using Elearning.Models.UserCustomer;
using Elearning.Services.UserCustomer;
using Elearning.Model.Models.UserAdmins;

namespace Elearning.Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    [NTSAuthorize]
    public class UserAdminController : BaseApiController
    {
        private IUserAdminService userAdminService;

        public UserAdminController(IUserAdminService userAdminService)
        {
            this.userAdminService = userAdminService;
        }

        /// <summary>
        /// Tìm kiếm tài khoản admin
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<UserAdminResultModel>>> SearchUserAdmin([FromBody] UserAdminSearch searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await userAdminService.SearchUserAdmin(searchModel);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Tìm kiếm người dùng
        /// </summary>
        /// <param name="searchModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search-user")]
        public async Task<ActionResult<SearchBaseResultModel<UserFontEndResultModel>>> SearchUser([FromBody] UserFondEndSearchModel searchModel)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            apiResultModel.Data = await userAdminService.SearchUser(searchModel);

            return Ok(apiResultModel);
        }


        /// <summary>
        /// Lấy dữ liệu tài khoản admin theo id
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("get-user-by-id/{UserId}")]
        public async Task<ActionResult<ApiResultModel<UserAdminModel>>> GetUserAdminById([FromRoute]string UserId)
        {
            ApiResultModel<UserAdminModel> apiResultModel = new ApiResultModel<UserAdminModel>
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await userAdminService.GetUserAdminById(UserId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Khóa tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("UserAdminLock/{userId}")]
        public async Task<ActionResult<ApiResultModel>> UserAdminLock(string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await userAdminService.UserAdminLock(Request, userId, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Mở khóa tài khoản
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("UserAdminUnLock/{userId}")]
        public async Task<ActionResult<ApiResultModel>> UserAdminUnLock(string userId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await userAdminService.UserAdminUnLock(Request, userId, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Cập nhật tài khoản
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        [HttpPut]
        [Route("update/{id}")]
        public async Task<ActionResult<ApiResultModel>> UpdateAdminUser([FromBody] UserCreateModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await userAdminService.UpdateAdminUser(Request, model, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thêm mới tài khoản admin
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ApiResultModel>> CreateAdminUser([FromBody] UserCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await userAdminService.CreateAdminUser(Request, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteUserAdmin(string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            await userAdminService.DeleteUserAdmin(Request, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("GetGroupPermissionById/{groupUserId}")]
        public async Task<ActionResult<ApiResultModel>> GetGroupPermissionById([FromRoute]string groupUserId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = userAdminService.GetGroupPermissionById(groupUserId);

            return Ok(apiResultModel);
        }

        [HttpGet]
        [Route("GetGroupPermission/{UserId}")]
        public async Task<ActionResult<ApiResultModel>> GetGroupPermission(string UserId)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = userAdminService.GetGroupPermission(UserId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Thay đổi mật khẩu
        /// </summary>
        /// <param name="model">Dữ liệu truyền lên api</param>
        /// <param name="id">id tài khoản</param>
        /// <returns></returns>
        [HttpPut]
        [Route("change-password/{id}")]
        public async Task<ActionResult<ApiResultModel>> ChangePassword([FromBody] ChangePasswordModel model, string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await userAdminService.ChangePassword(Request, model, id, GetUserIdByRequest());

            return Ok(apiResultModel);
        }

        [HttpPut]
        [Route("changepass")]
        public async Task<ActionResult<ApiResultModel>> ChangePassword([FromBody] ChangePass model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            await userAdminService.ChangePass(Request, model, GetUserIdByRequest());

            return Ok(apiResultModel);
        }
    }
}