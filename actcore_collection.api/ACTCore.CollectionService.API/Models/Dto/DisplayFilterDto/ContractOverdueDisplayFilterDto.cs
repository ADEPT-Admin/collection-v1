using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayFilterDto
{
    public class ContractOverdueDisplayFilterDto
    {
        [ColumnWidth(80)]
        public int? No { get; set; }
        public string Type { get; set; }

        [ColumnWidth(150)]
        public decimal? Amount { get; set; }
    }
}