using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class SysItemDto
    {
        public string ItemId { get; set; }
        public string ParentId { get; set; }
        public LanguageValue ItemName { get; set; }
        public string RouteName { get; set; }
        public int? ItemOrder { get; set; }
        public int? ItemLevel { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; set; }
        public List<SysItemAccessRightDto> ItemAccessRights { get; set; }
        public List<SysUserItemFavoriteDto> UserItemFavorites { get; set; }
    }
}
