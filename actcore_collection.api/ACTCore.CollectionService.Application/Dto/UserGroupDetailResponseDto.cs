namespace ACTCore.CollectionService.Application.Dto
{
    public class UserGroupDetailResponseDto
    {
        public Guid UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }

        public List<UserGroupAccessDto> UserGroupAccesses { get; set; } = new();
        public List<ItemAccessRightDto> ItemAccessRights { get; set; } = new();
    }
}