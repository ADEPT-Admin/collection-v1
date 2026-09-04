using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysItemResponseDto
    {
        public string ItemId { get; set; }
        public string ParentId { get; set; }
        public LanguageValue ItemName { get; set; }
        public string RouteName { get; set; }
        public int? ItemOrder { get; set; }
        public int ItemLevel { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; set; }
    }
}
