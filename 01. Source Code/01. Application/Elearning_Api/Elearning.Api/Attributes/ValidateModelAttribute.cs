using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elearning.Models;

namespace Elearning.Api.Attributes
{
    public class ValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<string> errors = new List<string>();
                foreach (var item in context.ModelState.ToList())
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errors.Add(error.ErrorMessage);
                    }
                }

                var errorModel = new ApiResultModel<List<string>>()
                {
                    Message = errors,
                    StatusCode = ApiResultConstants.StatusCodeValidateError
                };

                var serializerSettings = new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    Formatting = Formatting.Indented
                };

                var contentResult = JsonConvert.SerializeObject(errorModel, serializerSettings);

                context.HttpContext.Response.ContentType = "application/json";
                context.HttpContext.Response.StatusCode = StatusCodes.Status200OK;
                context.HttpContext.Response.WriteAsync(contentResult);

                context.Result = new OkResult();
            }
        }
    }
}
