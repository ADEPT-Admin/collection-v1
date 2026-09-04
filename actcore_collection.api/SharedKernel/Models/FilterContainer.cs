using SharedKernel.Templates;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SharedKernel.Models
{
    public class FilterContainer
    {
        [JsonExtensionData]
        public Dictionary<string, JsonElement> DynamicFiltersRaw { get; set; } // use class FilterItem to be the template of DynamicFilters

        [JsonIgnore]
        public Dictionary<string, FilterItem> DynamicFilters
        {
            get
            {
                var dict = new Dictionary<string, FilterItem>();
                if (DynamicFiltersRaw != null)
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };
                    foreach (var kvp in DynamicFiltersRaw)
                    {
                        var filterItem = kvp.Value.Deserialize<FilterItem>(options);
                        if (filterItem != null)
                        {
                            dict[kvp.Key] = filterItem;
                        }
                    }
                }
                return dict;
            }
        }
    }
}
