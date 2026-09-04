namespace ACTCore.CollectionService.Application.Dto
{
    public class PermissionAllowanceDto
    {
        public bool AllowAccess { get; set; }

        public bool AllowView { get; set; }

        public bool AllowNew { get; set; }

        public bool AllowEdit { get; set; }

        public bool AllowDelete { get; set; }
    }
}
