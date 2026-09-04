using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using SharedKernel.CommonConstants;
using System.Net;


namespace ACTCore.CollectionService.API.Middlewares
{
    public class ActiveSessionMiddleware
    {
        private readonly RequestDelegate _next;

        public ActiveSessionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, ActiveSessionService activeSessionService,
            AuthService authService, IAuthLogService authLog, IServiceProvider serviceProvider)
        {
            using (IServiceScope scope = serviceProvider.CreateScope())
            {
                LanguageService languageService = scope.ServiceProvider.GetRequiredService<LanguageService>();
                if (context.User.Identity?.IsAuthenticated == true)
                {
                    var sessionIdClaim = context.User.FindFirst("sessionid")?.Value;
                    if (Guid.TryParse(sessionIdClaim, out var sessionId))
                    {
                        var isActive = await activeSessionService.IsActiveAsync(sessionId);
                        if (isActive)
                        {
                            await activeSessionService.UpdateLastActivityAsync(sessionId);
                        }
                        else
                        {
                            var endpoint = context.GetEndpoint();
                            var routePattern = endpoint?.Metadata.GetMetadata<Microsoft.AspNetCore.Routing.RouteNameMetadata>()?.RouteName;
                            var routeValues = context.Request.RouteValues;

                            var controller = routeValues["controller"]?.ToString();
                            var action = routeValues["action"]?.ToString();

                            if (controller == "Auth" && action == "Logout")
                            {
                                // In the case of repeated logout with expired session, no need to do anything.
                                var response = new APIResponse
                                {
                                    StatusCode = HttpStatusCode.OK,
                                    Status = true,
                                    Message = await LangHelper.GetResponseMsgAsync(languageService, Message.Msg_LogoutSuccessful)
                                };
                                context.Response.StatusCode = StatusCodes.Status200OK;
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                                return;
                            }
                            else
                            {
                                await activeSessionService.RevokeSessionAsync(sessionId, "Session expired");
                                await authLog.UpdateAuthLogAsync(sessionId, AuthLogStatus.SESSION_EXPIRED);
                                await authService.MarkTokensInvalidBySessionId(sessionId);

                                var response = new APIResponse
                                {
                                    StatusCode = HttpStatusCode.Unauthorized,
                                    Status = false,
                                    Message = await LangHelper.GetResponseMsgAsync(languageService, Message.Msg_SessionExpired)
                                };

                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                                context.Response.Headers["WWW-Authenticate"] =
                                    "Bearer error=\"invalid_token\", error_description=\"Session expired\"";
                                context.Response.ContentType = "application/json";
                                await context.Response.WriteAsync(System.Text.Json.JsonSerializer.Serialize(response));
                                return;
                            }
                        }
                    }
                }
            }
            await _next(context);
        }
    }
}
