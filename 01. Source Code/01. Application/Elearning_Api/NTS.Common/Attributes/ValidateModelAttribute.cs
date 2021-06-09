using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NTS.Common.Attributes
{
    public class NtsValidateModelAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                List<NtsErrorValidateModel> errors = new List<NtsErrorValidateModel>();
                foreach (var item in context.ModelState.ToList())
                {
                    foreach (var error in item.Value.Errors)
                    {
                        errors.Add(new NtsErrorValidateModel
                        {
                            Message = error.ErrorMessage
                        });
                    }
                }

                var errorModel = new NtsApiResultModel<List<NtsErrorValidateModel>>()
                {
                    Data = errors,
                    StatusCode = NtsApiResultConstants.StatusCodeValidateError
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
