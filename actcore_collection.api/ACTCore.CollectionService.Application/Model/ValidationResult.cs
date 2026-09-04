using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Model
{
    public class ValidationResultModel
    {
        public bool IsValid { get; set; }
        public LanguageValue Message { get; set; }
        public LanguageValue WarningMessage { get; set; }

        public static ValidationResultModel Success()
        => new ValidationResultModel { IsValid = true };

        public static ValidationResultModel Fail(LanguageValue message)
            => new ValidationResultModel { IsValid = false, Message = message };

    public static ValidationResultModel Confirmation(LanguageValue message, LanguageValue warningMessage = null)
        => new ValidationResultModel
        {
            IsValid = false,
            Message = message,
            WarningMessage = warningMessage
        };
    }
}