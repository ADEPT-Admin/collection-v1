namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorResponseDto
    {
        public Guid CollectorId { get; set; }

        public Guid UserId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public Guid ColRoleId { get; set; }

        public ColRoleDto ColRole { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }
    }
}
