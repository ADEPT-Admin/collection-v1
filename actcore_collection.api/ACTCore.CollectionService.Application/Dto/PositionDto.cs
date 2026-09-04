using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class PositionDto
    {
        public int PositionId { get; set; }
        public string PositionCode { get; set; }
        public LanguageValue NameLanguage { get; set; }
    }
}
