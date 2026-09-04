using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class SysPolicyValidatePwdPolicyResponseDto
    {
        public bool Valid { get; set; }
        public LanguageValue Message { get; set; }
    }
}
