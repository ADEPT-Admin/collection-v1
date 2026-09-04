namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColTeamListResponseDto
    {
        public Guid ColTeamId { get; set; }

        public string ColTeamCode { get; set; }

        public string ColTeamName { get; set; }

        public int Capacity { get; set; }
    }
}
