using ACTCore.CollectionService.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Application.Dto
{
    public class LanguageJsonDto
    {
        public string Key { get; set; }
        public LanguageValue Value { get; set; }

        public LanguageValue DefaultValue { get; set; }

        public int? Ordering { get; set; }
    }
}
