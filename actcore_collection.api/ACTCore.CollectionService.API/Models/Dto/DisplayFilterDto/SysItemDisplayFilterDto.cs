namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class SysItemDisplayFilterDto
    {
        public string ItemNameEn { get; set; }
        public string ItemNameTh { get; set; }
        public string RouteName { get; set; }
        public int? ItemOrder { get; set; }
        public int? ItemLevel { get; set; }
        public string ToolTip { get; set; }
        public bool IsActive { get; set; }
    }
}
