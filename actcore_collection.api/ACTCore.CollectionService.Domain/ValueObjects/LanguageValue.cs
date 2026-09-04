using System.Text.Json.Serialization;

namespace ACTCore.CollectionService.Domain.ValueObjects;

public class LanguageValue
{
    [JsonPropertyName("en")]
    public string En { get; set; }

    [JsonPropertyName("th")]
    public string Th { get; set; }

    public LanguageValue Format(params object[] args)
    {
        return new LanguageValue
        {
            En = En != null ? string.Format(En, args) : null,
            Th = Th != null ? string.Format(Th, args) : null
        };
    }
}
