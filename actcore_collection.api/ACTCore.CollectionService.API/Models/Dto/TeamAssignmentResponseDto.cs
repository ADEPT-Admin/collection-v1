namespace ACTCore.CollectionService.API.Models.Dto
{
    public class TeamAssignmentResponseDto
    {
        public Guid AssignmentId { get; set; }

        public Guid ColTeamId { get; set; }

        public string ColTeamCode { get; set; }

        public string ColTeamName { get; set; }
        
        public string ColRoleName { get; set; }

        public int? TeamCapacity { get; set; }

        public Guid CollectorId { get; set; }

        public string CollectorEmpId { get; set; }

        public string CollectorEmpName { get; set; }

        public bool IsSupervisor { get; set; }

        public int Capacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
