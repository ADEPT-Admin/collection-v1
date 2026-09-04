using ACTCore.CollectionService.API.Filters;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.Application.Interface;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;

namespace ACTCore.CollectionService.API.Controllers.BaseController
{
    [ApiController]
    [Route("api/[controller]")]
    [ValidateModelState]
    public abstract class BaseApiController : ControllerBase
    {
        private readonly IErrorLogService _errorLogService;

        protected ClaimsPrincipal CurrentUser => HttpContext.User;
        protected CurrentUserInfo CurrentUserInfo => new CurrentUserInfo
        {
            UserId = Guid.TryParse(CurrentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var uid) ? uid : Guid.Empty,
            UserName = CurrentUser.FindFirst(ClaimTypes.Name)?.Value,
            Roles = CurrentUser.FindFirst(ClaimTypes.Role)?.Value,
            TokenId = CurrentUser.FindFirst(JwtRegisteredClaimNames.Jti)?.Value,
            SessionId = Guid.TryParse(CurrentUser.FindFirst("sessionid")?.Value, out var sid) ? sid : Guid.Empty,
            EmployeeId = CurrentUser.FindFirst("empid")?.Value
        };

        protected BaseApiController(IErrorLogService errorLogService)
        {
            _errorLogService = errorLogService;
        }

        protected async Task<ActionResult> HandleExceptionAsync(APIResponse response, Exception ex)
        {
            try
            {
                var errorLog = new ErrorLog
                {
                    UserId = CurrentUserInfo?.UserId,
                    UserName = CurrentUserInfo?.UserName,
                    UserGroup = CurrentUserInfo?.Roles,
                    RequestPath = HttpContext.Request?.Path,
                    HttpMethod = HttpContext.Request?.Method,
                    IPAddress = HttpContext.Connection?.RemoteIpAddress?.ToString(),
                    UserAgent = HttpContext.Request?.Headers["User-Agent"].ToString(),
                    InnerException = (ex.InnerException != null) ? ex.InnerException.Message : null,
                    ErrorMessage = ex.Message,
                    StackTrace = ex.StackTrace,
                };

                await _errorLogService.ErrrorLogAsync(errorLog);

                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Status = false;
                response.TraceId = errorLog.TraceId.ToString();

                // Append exception and inner exception messages to ErrorMessages
                response.ErrorMessages ??= new List<string>();
                response.ErrorMessages.Add(ex.Message);
                var inner = ex.InnerException;
                while (inner != null)
                {
                    response.ErrorMessages.Add(inner.Message);
                    inner = inner.InnerException;
                }

                return new ObjectResult(response)
                {
                    StatusCode = (int)HttpStatusCode.InternalServerError
                };
            }
            catch
            {
                throw;
            }
        }
    }
}
