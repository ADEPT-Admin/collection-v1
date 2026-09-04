namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class ColTeamListPagedResponseDto
    {
        public Guid ColTeamId { get; set; }

        public string ColTeamCode { get; set; }

        public string ColTeamName { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }

        public string IsActive { get; set; }
    }
}
