using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace NTS.Common.Attributes
{
    public class NtsAllowPermissionAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public string Permissions { get; set; }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var claimPermissions = context.HttpContext.User.Claims
                                          .FirstOrDefault(c => c.Type.Equals("Permissions"));
            if (!string.IsNullOrEmpty(claimPermissions?.Value))
            {
                if (!string.IsNullOrEmpty(this.Permissions)) 
                {
                    var hasPermission = claimPermissions.Value
                                                        .Split(",")
                                                        .Intersect(this.Permissions.Split(",").Select(p => p.Trim()).ToArray());
                    if (hasPermission.Any())
                    {
                        return;
                    }
                }
            }
            context.Result = new ForbidResult();
        }
    }
}
