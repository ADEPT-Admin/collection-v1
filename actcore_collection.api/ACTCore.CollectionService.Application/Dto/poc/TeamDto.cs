namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class TeamDto
    {
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamNameEn { get; set; }
        public bool IsActive { get; set; }
        public List<EmployeeProfileDto> EmployeeProfiles { get; set; }
        public List<ColTeamGroupDto> ColTeamGroups { get; set; }
        public List<ColTeamMappingAreaDto> ColTeamMappingAreas { get; set; }
    }

    public class TeamCreateDto
    {
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamNameEn { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeamUpdateDto
    {
        public int TeamId { get; set; }
        public string TeamCode { get; set; }
        public string TeamName { get; set; }
        public string TeamNameEn { get; set; }
        public bool IsActive { get; set; }
    }

    public class TeamIdRequestDto
    {
        public int TeamId { get; set; }
    }
}