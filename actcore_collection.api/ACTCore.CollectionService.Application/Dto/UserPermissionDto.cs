namespace ACTCore.CollectionService.Application.Dto
{
    public class UserPermissionDto
    {
        public Guid UserID { get; set; }
        public bool IsCollectorSupervisor { get; set; }
        public EmployeeProfileDto employee { get; set; } = new();
        public List<UserGroupDto> UserGroup { get; set; } = new();
        //public CollectionRoleDto Role { get; set; } = new();
        public List<UserPermissionItemAccess> MenuItem { get; set; } = new();
    }
}
