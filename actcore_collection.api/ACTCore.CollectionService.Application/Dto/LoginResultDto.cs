using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class LoginResultDto
    {
        public SysUser User;
        public LoginResponseDto Response;
    }

    public class LoginResponseDto
    {
        public string AccessToken { get; set; } = string.Empty;
        public string RefreshToken { get; set; } = string.Empty;
        public bool IsSuccess { get; set; } = false;
        public LanguageValue Message { get; set; }
    }
}
