using ACTCore.CollectionService.API.Models.Dto.poc;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserResponseDto
    {
        public Guid UserId { get; set; }
        public string UserName { get; set; }
        public EmployeeResponseDto Employee { get; set; }
        public bool IsActive { get; set; }
        public bool IsLockUser { get; set; }
        public bool IsNewUser { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpireDate { get; set; }
        public DateTime? LastSignOnDate { get; set; }
        public DateTime? LastChangePasswordDate { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public List<UserGroupResponseDto> UserGroups { get; set; }
    }
}