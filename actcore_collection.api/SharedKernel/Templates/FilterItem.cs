
using System.Text.Json;

namespace SharedKernel.Templates
{
    public class FilterItem
    {
        public string Operator { get; set; }  // เช่น "=", "between"
        public string Type { get; set; }      // เช่น "Number", "DateTime", "String", "Enum"
        public object Value { get; set; }     // ใช้ได้ทั้ง "Number","DateTime" "String", "Enum"
        public BetweenValue Values { get; set; } // ใช้เฉพาะ "Date" กรณี between

        public string returnStringValue()
        {
            string value = null;
            if (Value is JsonElement jsonElement)
            {
                switch (jsonElement.ValueKind)
                {
                    case JsonValueKind.String:
                        value = jsonElement.GetString();
                        break;
                    case JsonValueKind.Number:
                        value = jsonElement.TryGetInt64(out var l) ? l.ToString() :
                                jsonElement.TryGetDouble(out var d) ? d.ToString() : null;
                        break;
                    case JsonValueKind.True:
                    case JsonValueKind.False:
                        value = jsonElement.GetBoolean().ToString();
                        break;
                    default:
                        value = jsonElement.ToString();
                        break;
                }
            }
            else if (Value != null)
            {
                value = Value.ToString();
            }
            return value;
        }
    }
}
