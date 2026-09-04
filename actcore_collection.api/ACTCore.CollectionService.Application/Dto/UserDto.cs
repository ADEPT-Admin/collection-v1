namespace ACTCore.CollectionService.Application.Dto
{
    public class UserDto
    {
        public string UserId { get; set; }
        public string EmployeeId { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockUser { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsNewUser { get; set; }
        public int? MissingHit { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        //public List<UserGroupDto> UserGroups { get; set; } = new();
    }
}
