using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ContractOverdueReponseDto
    {
        public int? No { get; set; }
        public LanguageValue Type { get; set; }

        public decimal? Amount { get; set; }
    }
}
