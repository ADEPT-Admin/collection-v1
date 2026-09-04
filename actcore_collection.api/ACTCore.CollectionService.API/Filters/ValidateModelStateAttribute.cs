using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SharedKernel.CommonConstants;
using System.Net;


namespace ACTCore.CollectionService.API.Filters
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            if (!context.ModelState.IsValid)
            {
                var languageService = context.HttpContext.RequestServices.GetRequiredService<ILanguageService>();
                var message = await LangHelper.GetResponseMsgAsync(languageService, Message.Msg_ValidationFailed);

                var errors = context.ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new APIResponse(HttpStatusCode.BadRequest, false, message, null, errors);

                context.Result = new BadRequestObjectResult(response);
                return;
            }
            await next();
        }
    }
}
