using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using Microsoft.Extensions.Configuration;

namespace ACTCore.CollectionService.Application.Security
{
    public class ActiveSessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public ActiveSessionService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public async Task CreateSessionAsync(Guid sessionId, Guid userId, string userName,
            string ipAddress, string userAgent, string deviceInfo = null, string location = null)
        {
            double sessionExpiresHours = Convert.ToDouble(_configuration["ApiSettings:SessionExpiresHours"]);
            var sessionExpiry = DateTime.Now.AddHours(sessionExpiresHours);

            var session = new ActiveSession
            {
                SessionId = sessionId,
                UserId = userId,
                LoginTime = DateTime.Now,
                LastActivityTime = DateTime.Now,
                SessionExpiryTime = sessionExpiry,
                IpAddress = ipAddress,
                UserAgent = userAgent,
                DeviceInfo = deviceInfo,
                Location = location,
                RefreshTokenHash = string.Empty,
                IsRevoked = false
            };

            await _unitOfWork.Repository<ActiveSession>().AddAsync(session);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateRefreshTokenHashAsync(Guid sessionId, string refreshTokenHash)
        {
            var sessionRepo = _unitOfWork.Repository<ActiveSession>();
            var session = await sessionRepo.GetAsync(s => s.SessionId == sessionId, asNoTracking: false);
            if (session != null)
            {
                session.RefreshTokenHash = refreshTokenHash;
                session.LastRefreshedAt = DateTime.Now;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task UpdateLastActivityAsync(Guid sessionId)
        {
            var sessionRepo = _unitOfWork.Repository<ActiveSession>();
            var session = await sessionRepo.GetAsync(s => s.SessionId == sessionId, asNoTracking: false);
            if (session != null)
            {
                session.LastActivityTime = DateTime.Now;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task RevokeSessionAsync(Guid sessionId, string reason = "User logout")
        {
            var sessionRepo = _unitOfWork.Repository<ActiveSession>();
            var session = await sessionRepo.GetAsync(s => s.SessionId == sessionId, asNoTracking: false);
            if (session != null)
            {
                session.IsRevoked = true;
                session.RevokedReason = reason;
                await _unitOfWork.SaveChangesAsync();
            }
        }

        public async Task<bool> IsActiveAsync(Guid sessionId)
        {
            var session = await _unitOfWork.Repository<ActiveSession>()
                .GetAsync(s => s.SessionId == sessionId, asNoTracking: true);

            return session != null && session.IsActive;
        }

        public async Task RevokeAllSessionsAsync(Guid userId, string reason = "Force logout")
        {
            var sessionRepo = _unitOfWork.Repository<ActiveSession>();
            var sessions = await sessionRepo.GetAllAsync(s => s.UserId == userId && !s.IsRevoked, asNoTracking: false);
            if (sessions != null)
            {
                foreach (var session in sessions)
                {
                    session.IsRevoked = true;
                    session.RevokedReason = reason;
                }
                await _unitOfWork.SaveChangesAsync();
            }
        }
    }
}
