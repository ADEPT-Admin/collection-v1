using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.Domain.ValueObjects;
using Microsoft.AspNetCore.Diagnostics;
using Newtonsoft.Json;
using System.Net;

namespace ACTCore.CollectionService.API.Extensions
{
    public static class CustomExceptionExtensions
    {
        public static void HandleError(this IApplicationBuilder app, bool isDevelopment)
        {
            app.UseExceptionHandler(error =>
            {
                error.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";
                    var feature = context.Features.Get<IExceptionHandlerFeature>();
                    if (feature != null)
                    {
                        if (isDevelopment)
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new APIResponse
                            {
                                StatusCode = (HttpStatusCode)context.Response.StatusCode,
                                Status = false,
                                Message = new LanguageValue() { En = feature.Error.Message, Th = feature.Error.Message },
                                ErrorMessages = new List<string> { $"StackTrace:{feature.Error.StackTrace}" }
                            }));
                        }
                        else
                        {
                            await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                            {

                                StatusCode = (HttpStatusCode)context.Response.StatusCode,
                                Success = false,
                                Message = "An error occurred while processing your request.",//ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
                                ErrorMessages = new List<string> { feature.Error.Message }
                            }));
                        }
                    }
                });
            });
        }
    }
}
