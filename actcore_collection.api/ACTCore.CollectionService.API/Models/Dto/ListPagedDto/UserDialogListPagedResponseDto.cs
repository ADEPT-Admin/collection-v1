namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class UserDialogListPagedResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public string EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string UserGroup { get; set; }
        public string Email { get; set; }
        public string PhoneNo { get; set; }

        // public bool IsLockUser { get; set; }
        // public DateTime? EffectiveDate { get; set; }
        // public DateTime? ExpireDate { get; set; }
        // public DateTime? LastSignOnDate { get; set; }
        // public DateTime? LastChangePasswordDate { get; set; }
        // public string IsActive { get; set; }
    }
}
