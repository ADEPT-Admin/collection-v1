using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayFilterDto
{
    public class ColTeamDisplayFilterDto
    {
        [ColumnWidth(150)]
        public string ColTeamCode { get; set; }

        public string ColTeamName { get; set; }

        [ColumnWidth(150)]
        public int Capacity { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
