namespace ACTCore.CollectionService.Application.Dto
{
    public class SysItemAccessRightDto
    {
        public int UserGroupId { get; set; }
        public string ItemId { get; set; }
        public bool AllowNew { get; set; }
        public bool AllowSave { get; set; }
        public bool AllowDelete { get; set; }
        public bool AllowCancel { get; set; }
        public bool AllowQuery { get; set; }
        public bool AllowPrint { get; set; }
    }
}
