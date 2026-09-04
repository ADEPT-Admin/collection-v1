using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models
{
    public class ApiPaginationResponse : APIResponse
    {
        public ApiPaginationResponse()
        {
            ErrorMessages = new List<string>();
        }

        public ApiPaginationResponse(bool success, LanguageValue message, object data = null, List<string> errors = null)
        {
            Status = success;
            Message = message;
            Data = data;
            ErrorMessages = errors;
        }
    }
}
