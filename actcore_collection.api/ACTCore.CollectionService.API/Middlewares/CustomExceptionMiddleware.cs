using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Domain.ValueObjects;
using Newtonsoft.Json;
using SharedKernel.Models;
using System.Net;
using System.Security.Claims;

namespace ACTCore.CollectionService.API.Middlewares
{
    public class CustomExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IWebHostEnvironment _environment;

        public CustomExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
        {
            _next = next;
            _environment = environment;

        }

        public async Task InvokeAsync(HttpContext context, IErrorLogService logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, logger, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, IErrorLogService logger, Exception ex)
        {
            try
            {
                context.Response.Clear();
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                if (_environment.IsDevelopment())
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new APIResponse
                    {
                        StatusCode = (HttpStatusCode)context.Response.StatusCode,
                        Status = false,
                        Message = new LanguageValue() { En = ex.Message, Th = ex.Message },
                        ErrorMessages = new List<string> { $"StackTrace:{ex.StackTrace}" }
                    }));
                }
                else
                {
                    await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                    {

                        StatusCode = (HttpStatusCode)context.Response.StatusCode,
                        Success = false,
                        Message = "An error occurred while processing your request.",//ReasonPhrases.GetReasonPhrase(context.Response.StatusCode),
                        ErrorMessages = new List<string> { ex.Message }
                    }));
                }
            }
            catch { }
            finally
            {
                var errorLog = new ErrorLog
                {
                    UserId = context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value != null ?
                             Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : null,
                    UserName = context.User?.FindFirst(ClaimTypes.Name)?.Value,
                    UserGroup = context.User?.FindFirst(ClaimTypes.Role)?.Value,
                    RequestPath = context.Request?.Path,
                    HttpMethod = context.Request?.Method,
                    IPAddress = context.Connection?.RemoteIpAddress?.ToString(),
                    UserAgent = context.Request?.Headers["User-Agent"].ToString(),
                    ErrorMessage = ex.Message,
                    InnerException = (ex.InnerException != null) ? ex.InnerException.Message : null,
                    StackTrace = $"StackTrace:{ex.StackTrace}"
                };
                await logger.ErrrorLogAsync(errorLog);
            }
        }
    }
}

