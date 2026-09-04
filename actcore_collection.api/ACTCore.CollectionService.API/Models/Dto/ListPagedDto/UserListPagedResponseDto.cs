namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class UserListPagedResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserGroup { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? LastSignOnDate { get; set; }
        public string IsActive { get; set; }
    }
}
