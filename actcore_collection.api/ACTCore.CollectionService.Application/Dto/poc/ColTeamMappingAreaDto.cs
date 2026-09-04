namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class ColTeamMappingAreaDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string AreaId { get; set; }
        public TeamSimpleDto Team { get; set; }
        public AreaCodeDto Area { get; set; }
    }

    public class TeamSimpleDto
    {
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamNameEn { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColTeamMappingAreaCreateDto
    {
        public int TeamId { get; set; }
        public string AreaId { get; set; }
    }

    public class ColTeamMappingAreaUpdateDto
    {
        public int Id { get; set; }
        public int TeamId { get; set; }
        public string AreaId { get; set; }
    }

    public class ColTeamMappingAreaIdRequestDto
    {
        public int Id { get; set; }
    }
}