using ACTCore.CollectionService.Domain.ValueObjects;
using System.Net;
using System.Text.Json.Serialization;

namespace ACTCore.CollectionService.API.Models
{
    public class APIResponse
    {
        public APIResponse()
        {
            ErrorMessages = new List<string>();
        }
        [JsonPropertyOrder(0)]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyOrder(1)]
        public bool Status { get; set; } = false;

        [JsonPropertyOrder(2)]
        public LanguageValue Message { get; set; }

        [JsonPropertyOrder(3)]
        public bool IsRequireConfirmation { get; set; } = false;

        [JsonPropertyOrder(4)]
        public LanguageValue WarningMessages { get; set; }

        [JsonPropertyOrder(5)]
        public object? Data { get; set; }

        [JsonPropertyOrder(6)]
        public string TraceId { get; set; }

        [JsonPropertyOrder(99)]
        public List<string>? ErrorMessages { get; set; }

        public APIResponse(HttpStatusCode statusCode, bool success, LanguageValue message, object? data = null, List<string>? errors = null)
        {
            StatusCode = statusCode;
            Status = success;
            Message = message;
            Data = data;
            ErrorMessages = errors;
        }
    }
}
