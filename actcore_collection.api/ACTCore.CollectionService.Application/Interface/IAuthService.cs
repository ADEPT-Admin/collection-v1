using ACTCore.CollectionService.Application.Dto;

namespace ACTCore.CollectionService.Application.Interface
{
    public interface IAuthService
    {
        Task<LoginResultDto> Login(string userName, string password, Guid sessionId);
        Task<TokenResultDto> RefreshAccessToken(string accessToken, string refreshToken);
        Task RevokeRefreshToken(string accessToken, string refreshToken);
    }
}
