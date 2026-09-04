
using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class LanguageResponseDto
    {
        public string Key { get; set; }

        public LanguageValue Value { get; set; }

        public LanguageValue DefaultValue { get; set; }

        public int? Ordering { get; set; }
    }
}
