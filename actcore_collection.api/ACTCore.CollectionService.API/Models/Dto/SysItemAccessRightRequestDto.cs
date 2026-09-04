namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysItemAccessRightRequestDto
    {

        public string ItemId { get; set; }

        public bool AllowAccess { get; set; }
        public bool AllowView { get; set; }

        public bool AllowNew { get; set; }

        public bool AllowEdit { get; set; }

        public bool AllowDelete { get; set; }
    }
}
