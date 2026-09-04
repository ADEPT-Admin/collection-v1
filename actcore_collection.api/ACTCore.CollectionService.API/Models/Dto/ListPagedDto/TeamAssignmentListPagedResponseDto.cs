namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class TeamAssignmentListPagedResponseDto
    {
        public Guid AssignmentId { get; set; }

        public string ColTeamName { get; set; }

        public string CollectorEmpId { get; set; }

        public string CollectorEmpName { get; set; }

        public string IsSupervisor { get; set; }

        public int Capacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public string IsActive { get; set; }
    }
}
