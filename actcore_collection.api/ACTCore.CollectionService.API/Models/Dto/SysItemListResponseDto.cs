using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysItemListResponseDto
    {
        public string ItemId { get; set; }
        public string ParentId { get; set; }
        public string ItemNameEn { get; set; }
        public string ItemNameTh { get; set; }
        public string RouteName { get; set; }
        public int? ItemOrder { get; set; }
        public int ItemLevel { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        public string IsActive { get; set; }
    }
}
