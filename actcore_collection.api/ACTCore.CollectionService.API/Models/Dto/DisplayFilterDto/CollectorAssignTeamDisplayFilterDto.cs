using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class CollectorAssignTeamDisplayFilterDto
    {
        [ColumnWidth(120)]
        public string CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string ColRoleName { get; set; }
    }
}
