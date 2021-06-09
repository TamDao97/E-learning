using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Microsoft.Extensions.DependencyInjection;
using Elearning.Models;
using NTS.Common;
using NTS.Common.Resource;
using System.Linq;

namespace Elearning.Api.Attributes
{
    public class ApiHandleExceptionSystemAttribute : ExceptionFilterAttribute
    {
        public override void OnException(ExceptionContext context)
        {
            base.OnException(context);

            var errorModel = new ApiResultModel()
            {
                StatusCode = ApiResultConstants.StatusCodeError
            };

            if (context.Exception is NTSException)
            {
                var customError = context.Exception as NTSException;
                errorModel.Message.Add(customError.Message);
            }
            else
            {
                // Log exception to file
                var logger = context.HttpContext.RequestServices.GetRequiredService<ILoggerFactory>().CreateLogger("Exception system");
                logger.LogError(context.Exception.Message);
                logger.LogError(context.Exception.StackTrace);

                errorModel.Message.Add(ResourceUtil.GetResourcesNoLag(MessageResourceKey.ERR0001));
            }

            var serializerSettings = new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                Formatting = Formatting.Indented
            };

            var contentResult = JsonConvert.SerializeObject(errorModel, serializerSettings);

            context.HttpContext.Response.ContentType = "application/json";
            context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
            context.HttpContext.Response.WriteAsync(contentResult);
            context.ExceptionHandled = true;
        }
    }
}
