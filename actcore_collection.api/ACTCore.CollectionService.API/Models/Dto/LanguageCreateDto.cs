using ACTCore.CollectionService.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class LanguageCreateDto
    {
        [Required]
        public string Key { get; set; }
        
        [Required]
        public LanguageValue Value { get; set; }

    }
}
