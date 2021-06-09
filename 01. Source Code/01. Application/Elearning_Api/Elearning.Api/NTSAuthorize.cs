using Elearning.Models;
using Elearning.Services.Authen;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using NTS.Common.Users;
using System.Linq;
using System.Security.Claims;

namespace Elearning.Api
{
    public class NTSAuthorize : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            ApiResultModel apiResultModel = new ApiResultModel();

            var user = context.HttpContext.User.Claims.FirstOrDefault(c => c.Type.Equals(ClaimTypes.Name));
            if (user != null)
            {
                var service = context.HttpContext.RequestServices.GetRequiredService<IAuthenService>();
                var isAuthorized = service.IsTokenAlive(user.Value);
                if (!isAuthorized)
                {
                    context.Result = new ObjectResult(apiResultModel)
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }
            }

            return;
        }
    }
}
