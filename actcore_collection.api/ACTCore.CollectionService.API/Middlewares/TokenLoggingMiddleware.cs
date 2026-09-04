using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using SharedKernel.CommonConstants;
using System.IdentityModel.Tokens.Jwt;


namespace ACTCore.CollectionService.API.Middlewares
{
    public class TokenLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<TokenLoggingMiddleware> _logger;

        public TokenLoggingMiddleware(RequestDelegate next, ILogger<TokenLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, IAuthLogService authLog, ActiveSessionService userActiveSessionService)
        {
            var authHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            Guid? userId = null;
            string username = null;
            Guid sessionId = Guid.Empty;
            string tokenStatus = "Invalid";
            string failReason = "";
            DateTime checkedTime = DateTime.Now;

            if (!string.IsNullOrEmpty(authHeader) && authHeader.StartsWith("Bearer "))
            {
                var token = authHeader.Substring("Bearer ".Length).Trim();
                try
                {
                    var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(token);
                    var exp = jwtToken.Claims.FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Exp)?.Value;
                    username = jwtToken.Claims.FirstOrDefault(c => c.Type == "unique_name")?.Value;
                    var sessionIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == "sessionid")?.Value;
                    if (Guid.TryParse(sessionIdStr, out var parsedSessionId))
                        sessionId = parsedSessionId;

                    var userIdStr = jwtToken.Claims.FirstOrDefault(c => c.Type == "nameid")?.Value;
                    if (Guid.TryParse(userIdStr, out var parsedUserId))
                        userId = parsedUserId;

                    DateTime? expDate = null;
                    if (exp != null && long.TryParse(exp, out var expUnix))
                    {
                        expDate = DateTimeOffset.FromUnixTimeSeconds(expUnix).UtcDateTime;
                    }

                    if (expDate != null && expDate < DateTime.UtcNow)
                    {
                        tokenStatus = AuthLogStatus.TOKEN_EXPIRED;
                        failReason = $"Token expired at {expDate:yyyy-MM-dd HH:mm:ss}";
                        _logger.LogWarning("Token expired for user {User} at {Time}", username ?? "Unknown", checkedTime);

                        await authLog.AddAuthLogAsync(
                            userId,
                            username ?? "Unknown",
                            tokenStatus,
                            failReason,
                            sessionId
                        );
                    }
                }
                catch (Exception ex)
                {
                    tokenStatus = AuthLogStatus.TOKEN_INVALID;
                    failReason = $"Invalid token format: {ex.Message}";
                    _logger.LogError(ex, "Invalid token format");

                    await authLog.AddAuthLogAsync(
                        userId,
                        username ?? "Unknown",
                        tokenStatus,
                        failReason,
                        sessionId
                    );
                }
            }
            await _next(context);
        }
    }
}
