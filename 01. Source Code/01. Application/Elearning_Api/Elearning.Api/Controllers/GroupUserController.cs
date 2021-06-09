using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NTS.Model.GroupUser;
using Elearning.Api.Attributes;
using Elearning.Models;
using Elearning.Models.Base;
using Elearning.Models.GroupUser;
using Elearning.Services.GroupUsers;
using Microsoft.AspNetCore.Authorization;

namespace Elearning.Api.Controllers
{
    [ApiController]
    [ValidateModel]
    [Route("api/group-users")]
    //[AllowPermissionAttribute(Permissions="")]
    [NTSAuthorize]
    public class GroupUserController : BaseApiController
    {
        private readonly IGroupUserService groupUser;
        public GroupUserController(IGroupUserService groupUserService)
        {
            groupUser = groupUserService;
        }
        /// <summary>
        /// Tìm kiếm nhóm  quyền
        /// </summary>
        /// <param name="modelSearch"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<SearchBaseResultModel<GroupUserResultModel>>> SearchGroupUser([FromBody] GroupUserSearchModel modelSearch)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = await groupUser.SearchGroupUser(modelSearch);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy thông tin nhóm quyền theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public async Task<ActionResult<ActionResult<GroupFunctionInfoModel>>> GetGroupUserById([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await groupUser.GetGroupFunctionInfo(id, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Thêm nhóm quyền
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ApiResultModel>> CreateGroupUser([FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await groupUser.CreateGroupFunction(model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Cập nhập nhóm quyền
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]


        public async Task<ActionResult<ApiResultModel>> UpdateGroupUser([FromRoute] string id, [FromBody] GroupFunctionCreateModel model)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            string userId = GetUserIdByRequest();
            await groupUser.UpdateGroupFunction(id, model, userId);

            return Ok(apiResultModel);
        }
        /// <summary>
        /// Xóa nhóm quyền
        /// </summary>
        /// <param name="groupUserId"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        public async Task<ActionResult<ApiResultModel>> DeleteGroupUser([FromRoute] string id)
        {
            ApiResultModel apiResultModel = new ApiResultModel
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await groupUser.DeleteGroupFunctionById(id, userId);
            return Ok(apiResultModel);
        }
        /// <summary>
        /// Lấy danh sách quyền theo nhóm quyền
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("group-functions-functions")]
        [HttpGet]
        public async Task<ActionResult> GetFunctionByGroupFunctions(string groupId)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await groupUser.GetFunctionByGroupFunctions(groupId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách quyền theo user và nhóm quyền
        /// </summary>
        /// <param name="groupId"></param>
        /// <returns></returns>
        [Route("GetFuntionByGroupAndUser")]
        [HttpGet]
        public async Task<ActionResult> GetFuntionByGroupAndUser(string groupId)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = await groupUser.GetFuntionByGroupAndUser(groupId, userId);

            return Ok(apiResultModel);
        }


        /// <summary>
        /// Lấy quyền của nhóm
        /// </summary>
        /// <param name="groupId">id group user truyên lên</param>
        /// <returns></returns>
        [Route("group-permission/{groupId}")]
        [HttpGet]
        public async Task<ActionResult> GetGroupPermission([FromRoute] string groupId)
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };

            apiResultModel.Data = groupUser.GetGroupPermission(groupId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy danh sách quyền
        /// </summary>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        [Route("permisstion")]
        [HttpGet]
        public async Task<ActionResult> GetPermisstions()
        {
            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            apiResultModel.Data = groupUser.GetPermisstions(userId);

            return Ok(apiResultModel);
        }

        /// <summary>
        /// Lấy các quyền của nhóm quyền cập nhật cho các user có 1 trong các quyền thuộc nhóm.
        /// </summary>
        /// <param name="groupId">Id nhóm quyên</param>
        /// <returns></returns>
        [Route("accept-function/{groupId}")]
        [HttpPut]
        public async Task<ActionResult> AcceptFunction([FromRoute] string groupId)
        {

            ApiResultModel apiResultModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeSuccess
            };
            string userId = GetUserIdByRequest();
            await groupUser.AcceptFunction(groupId, userId);

            return Ok(apiResultModel);
        }

    }
}