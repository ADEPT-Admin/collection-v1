namespace ACTCore.CollectionService.Application.Dto
{
    public class UserGroupAccessDto
    {
        public Guid UserId { get; set; }
        public bool IsActive { get; set; }
        // Optionally include user info
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }
}
