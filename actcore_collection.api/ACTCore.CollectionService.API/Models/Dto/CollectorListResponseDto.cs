namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorListResponseDto
    {
        public string CollectorId { get; set; }

        public string EmployeeId { get; set; }

        public string EmployeeName { get; set; }

        public string ColRoleName { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }
    }
}
