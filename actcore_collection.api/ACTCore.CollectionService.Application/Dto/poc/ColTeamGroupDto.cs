namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class ColTeamGroupDto
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public int? WorkGroupId { get; set; }
        public string SupervisorId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColTeamGroupListingResponseDto
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public string TeamName { get; set; }
        public int? WorkGroupId { get; set; }
        public string WorkGroupName { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColTeamGroupCreateDto
    {
        public int? TeamId { get; set; }
        public int? WorkGroupId { get; set; }
        public string SupervisorId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColTeamGroupUpdateDto
    {
        public int Id { get; set; }
        public int? TeamId { get; set; }
        public int? WorkGroupId { get; set; }
        public string SupervisorId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class ColTeamGroupIdRequestDto
    {
        public int Id { get; set; }
    }
    public class ColTeamGroupSupervisorIdRequestDto
    {
        public string SupervisorId { get; set; }
    }
}