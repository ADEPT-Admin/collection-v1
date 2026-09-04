using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Interface;
using SharedKernel.Models;
using System.Security.Claims;
using System.Text;

namespace ACTCore.CollectionService.API.Middlewares
{
    public class ApiLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ApiLoggingMiddleware> _logger;

        public ApiLoggingMiddleware(RequestDelegate next, ILogger<ApiLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, IApiLogService apiLog)
        {
            var request = context.Request;

            // Enable buffering so we can read the request body multiple times
            request.EnableBuffering();

            string requestBody = string.Empty;
            if (request.ContentLength > 0 && request.Body.CanRead)
            {
                request.Body.Position = 0;
                using (var reader = new StreamReader(request.Body, Encoding.UTF8, leaveOpen: true))
                {
                    requestBody = await reader.ReadToEndAsync();
                    request.Body.Position = 0;
                }
            }

            // Swap the response body with a memory stream to capture it
            var originalBodyStream = context.Response.Body;
            var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            var startTime = DateTime.Now;

            var log = new ApiLog
            {
                UserId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value != null ? Guid.Parse(context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value) : null,
                UserName = context.User.FindFirst(ClaimTypes.Name)?.Value,
                UserGroup = context.User.FindFirst(ClaimTypes.Role)?.Value,

                RequestPath = request.Path,
                HttpMethod = request.Method,
                RequestBody = JsonMaskingHelper.MaskSensitiveData(requestBody.Replace("\r\n", "")),
                IPAddress = context.Connection.RemoteIpAddress?.ToString(),
                UserAgent = context.Request.Headers["User-Agent"].ToString(),
                Timestamp = startTime,
            };

            try
            {
                await _next(context);

                // Read the response body
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
                context.Response.Body.Seek(0, SeekOrigin.Begin);

                log.Status = "Success";
                log.ResponseBody = JsonMaskingHelper.MaskSensitiveData(responseBodyText.Replace("\r\n", ""));
                log.ResponseCode = context.Response.StatusCode;
            }
            catch (Exception ex)
            {
                log.Status = "Failed";
                log.ErrorMessage = ex.Message;
                throw;
            }
            finally
            {
                // Copy the contents of the new memory stream (which contains the response) to the original stream
                context.Response.Body.Seek(0, SeekOrigin.Begin);
                await responseBody.CopyToAsync(originalBodyStream);
                context.Response.Body = originalBodyStream;

                log.DurationSeconds = (int)(DateTime.Now - startTime).TotalSeconds;

                await apiLog.ApiLogAsync(log);

                responseBody.Dispose();
            }
        }
    }
}
