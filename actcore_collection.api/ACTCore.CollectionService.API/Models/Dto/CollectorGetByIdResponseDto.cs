namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorGetByIdResponseDto
    {
        public Guid CollectorGuid { get; set; } // from Collector.CollectorId
        public string CollectorId { get; set; } // from User.EmployeeId
        public string CollectorName { get; set; }
        public Guid ColRoleId { get; set; }
        public ColRoleDto ColRole { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }
        public bool IsActive { get; set; }
    }
}