using ACTCore.CollectionService.API.Models.Attributes;
using SharedKernel.CommonConstants;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class WorklistListingSupervisorDisplayFilterDto
    {
        [ColumnWidth(120)]
        public string ContractNo { get; set; }

        [ColumnWidth(200)]
        public string CustomerName { get; set; }

        [ColumnWidth(120)]
        public string AssetType { get; set; }

        [ColumnWidth(120)]
        public string ContractStatus { get; set; }

        [ColumnWidth(100)]
        public int? Bucket { get; set; }
        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? DueDate { get; set; }

        [ColumnWidth(120)]
        public int? DayPastDue { get; set; }

        [ColumnWidth(120)]
        public decimal? OverdueAmount { get; set; }

        [ColumnWidth(120)]
        public decimal? OutstandingBalance { get; set; }

        [ColumnWidth(200)]
        public string CollectorName { get; set; }

        [ColumnWidth(200)]
        public string ReassignBy { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? AssignDate { get; set; }

        [ColumnWidth(150)]
        public string FollowupStatus { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? LastFollowupDate { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? NextFollowupDate { get; set; }

        [DateFormat(CommonConstants.DateTimeFormat)]
        public DateTime? PromiseToPayDate { get; set; }
    }
}
