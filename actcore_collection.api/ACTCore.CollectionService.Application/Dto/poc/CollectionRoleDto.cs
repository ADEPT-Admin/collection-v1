namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class CollectionRoleDto
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CollectionRoleCreateDto
    {
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CollectionRoleUpdateDto
    {
        public int RoleId { get; set; }
        public string RoleCode { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }

    public class CollectionRoleIdRequestDto
    {
        public int RoleId { get; set; }
    }
}
