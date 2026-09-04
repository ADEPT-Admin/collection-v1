using System.Text.Encodings.Web;
using System.Text.Json;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class JsonMaskingHelper
    {
        // กำหนด field ที่ต้อง mask
        private static readonly HashSet<string> SensitiveFields = new(StringComparer.OrdinalIgnoreCase)
        {
            "password",
            "currentPassword",
            "newPassword",
            "accessToken",
            "refreshToken",
            "confirmPassword",
            "secret",
            "cardNumber"
        };

        public static string MaskSensitiveData(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return json;

            try
            {
                using var doc = JsonDocument.Parse(json);
                var masked = MaskElement(doc.RootElement);

                //return JsonSerializer.Serialize(masked, new JsonSerializerOptions { WriteIndented = true });
                return JsonSerializer.Serialize(masked, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping});
            }
            catch
            {
                // ถ้าไม่ใช่ JSON ก็ return เดิม
                return json;
            }
        }

        private static object? MaskElement(JsonElement element)
        {
            return element.ValueKind switch
            {
                JsonValueKind.Object => element.EnumerateObject()
                    .ToDictionary(
                        p => p.Name,
                        p => SensitiveFields.Contains(p.Name) ? MaskValue(p.Value) : MaskElement(p.Value)
                    ),

                JsonValueKind.Array => element.EnumerateArray()
                    .Select(MaskElement)
                    .ToList(),

                _ => element.ValueKind switch
                {
                    JsonValueKind.String => element.GetString(),
                    JsonValueKind.Number => element.GetDouble(),
                    JsonValueKind.True => true,
                    JsonValueKind.False => false,
                    JsonValueKind.Null => null,
                    _ => element.ToString()
                }
            };
        }

        private static string MaskValue(JsonElement value) =>
            value.ValueKind == JsonValueKind.String ? "******" : "0";
    }
}
