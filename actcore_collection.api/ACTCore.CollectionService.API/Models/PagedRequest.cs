using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models
{
    public class PagedRequest<TFilter>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string SortColumn { get; set; }
        public string SortDirection { get; set; }
        public TFilter Filters { get; set; }
        public string Mode { get; set; } = ModeType.Page;
    }
}
