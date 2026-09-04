using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using SharedKernel.CommonConstants;
using SharedKernel.Models;


namespace ACTCore.CollectionService.Application
{
    public class AuthLogService : IAuthLogService
    {
        private readonly AppDbContext _dbContext;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthLogService(AppDbContext dbContext, IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetTraceId()
        {
            return _httpContextAccessor.HttpContext?.Items["TraceId"]?.ToString() ?? Guid.NewGuid().ToString();
        }

        public async Task AddAuthLogAsync(Guid? userId, string userName, string status, string failReason, Guid sessionId)
        {
            var log = new AuthLog
            {
                UserId = userId,
                UserName = userName,
                TraceId = Guid.Parse(GetTraceId()),
                Status = status,
                FailReason = failReason,
                IpAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString(),
                UserAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString(),
                LoginTime = DateTime.Now,
                SessionId = sessionId
            };

            await _dbContext.AuthLogs.AddAsync(log);
            await _dbContext.SaveChangesAsync();
        }
        public async Task UpdateAuthLogAsync(Guid sessionId, string authLogStatus)
        {
            var log = await _dbContext.AuthLogs.FirstOrDefaultAsync(x => x.SessionId == sessionId && x.Status == AuthLogStatus.LOGIN_SUCCESS);
            if (log != null)
            {
                log.Status = authLogStatus;
                if (authLogStatus == AuthLogStatus.LOGOUT)
                {
                    log.LogoutTime = DateTime.Now;
                }
                else if (authLogStatus == AuthLogStatus.SESSION_EXPIRED)
                {
                    log.ExpiredTime = DateTime.Now;
                }
                _dbContext.AuthLogs.Update(log);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
