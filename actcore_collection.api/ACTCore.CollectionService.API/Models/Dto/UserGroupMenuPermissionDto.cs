using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserGroupMenuPermissionDto
    {
        public string ItemId { get; set; }
        public string ParentId { get; set; }
        public Guid? UserGroupId { get; set; }
        public LanguageValue ItemName { get; set; }
        public string RouteName { get; set; }
        public int ItemLevel { get; set; }
        public bool AllowAccess { get; set; }
        public bool AllowView { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowEdit { get; set; }
        public bool AllowDelete { get; set; }
    }
}
