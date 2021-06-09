using Microsoft.AspNetCore.Mvc;
using NTS.Common.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Elearning.Api.Controllers
{
    public class BaseApiController : ControllerBase
    {
        protected string GetUserIdByRequest()
        {
            //string accessToken = HttpContext.Request.Headers["Authorization"];

            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value;

            return signedInUserId;
        }

        protected string GetLearnerIdByRequest()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type.Equals(ClaimTypes.Name))?.Value;

            return signedInUserId;
        }

        protected string GetEmployeeIdByRequset()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type == "employeeId")?.Value;

            return signedInUserId;
        }

        protected string GetManageUnitRequest()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            string signedInUserId = claims.FirstOrDefault(cl => cl.Type == "managerUnitId")?.Value;

            return signedInUserId;
        }

        protected int GetLevelRequest()
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            int signedInUserId = Int32.Parse(claims.FirstOrDefault(cl => cl.Type == "level")?.Value);

            return signedInUserId;
        }

        protected bool CheckPermission(string functionCode)
        {
            var identity = (ClaimsIdentity)User.Identity;
            IEnumerable<Claim> claims = identity.Claims;
            var claimPermissions = claims.FirstOrDefault(cl => cl.Type.Equals("Permissions"));
            if (!string.IsNullOrEmpty(claimPermissions?.Value))
            {
                if (!string.IsNullOrEmpty(functionCode))
                {
                    var hasPermission = claimPermissions.Value
                                                        .Split(",")
                                                        .Intersect(new string[] { functionCode });
                    if (hasPermission.Any())
                    {
                        return true;
                    }
                }
            }
            return false;
        }
    }
}
