namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class ColTeamListAssignByTeamPagedResponseDto
    {
        public Guid AssignmentId { get; set; }

        public string CollectorId { get; set; }

        public string CollectorName { get; set; }

        public string ColRoleName { get; set; }

        public string Supervisor { get; set; }

        public int CollectorCapacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }
        
        public string IsActive { get; set; }
    }
}