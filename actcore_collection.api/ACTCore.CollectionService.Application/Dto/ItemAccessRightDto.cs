using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class ItemAccessRightDto
    {
        public string ItemId { get; set; }

        public string ParentId { get; set; }

        public LanguageValue ItemName { get; set; }

        public string RouteName { get; set; }

        public int ItemLevel { get; set; }

        public int? ItemOrder { get; set; }

        public string Icon { get; set; }

        public string ToolTip { get; set; }

        public bool AllowAccess { get; set; }

        public bool AllowView { get; set; }

        public bool AllowNew { get; set; }

        public bool AllowEdit { get; set; }

        public bool AllowDelete { get; set; }
    }
}
