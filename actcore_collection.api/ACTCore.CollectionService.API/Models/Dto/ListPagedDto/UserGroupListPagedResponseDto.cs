namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class UserGroupListPagedResponseDto
    {
        public Guid? UserGroupId { get; set; }
        public string UserGroupCode { get; set; }
        public string UserGroupName { get; set; }
        public string Description { get; set; } = null;
        public string IsActive { get; set; }
    }
}
