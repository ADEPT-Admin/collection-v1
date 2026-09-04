namespace ACTCore.CollectionService.API.Models.Dto.poc
{
    public class UserGroupResponseDto
    {
        public Guid? UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public string Description { get; set; } = null;
        public bool IsActive { get; set; }
    }
}
