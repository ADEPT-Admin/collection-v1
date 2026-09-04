namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class CollectorDto
    {
        public int Id { get; set; }
        public string CollectorId { get; set; }
        public int? ColTeamGroupId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
        public EmployeeProfileDto Collector { get; set; }
        public ColTeamGroupDto ColTeamGroup { get; set; }
    }

    public class CollectorCreateDto
    {
        public string CollectorId { get; set; }
        public int? ColTeamGroupId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class CollectorUpdateDto
    {
        public int Id { get; set; }
        public string CollectorId { get; set; }
        public int? ColTeamGroupId { get; set; }
        public int? Capacity { get; set; }
        public bool IsActive { get; set; }
    }

    public class CollectorIdRequestDto
    {
        public string CollectorId { get; set; }
    }
}