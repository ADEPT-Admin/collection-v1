namespace ACTCore.CollectionService.Application.Dto
{
    public class UserGroupDto
    {
        public Guid UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public string Description { get; set; }
        public bool IsActive { get; set; }
    }
}
