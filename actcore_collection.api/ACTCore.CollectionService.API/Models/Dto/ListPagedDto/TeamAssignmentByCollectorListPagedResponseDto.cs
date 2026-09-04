namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class TeamAssignmentByCollectorListPagedResponseDto
    {
        public string ColTeamName { get; set; }
        public string Supervisor { get; set; }
        public int CollectorCapacity { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public string IsActive { get; set; }
    }
}
