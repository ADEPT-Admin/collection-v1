using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class UnassignWorklistDisplayFilterDto
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

        [ColumnWidth(120)]
        public int? PaymentDueDate { get; set; }

        [ColumnWidth(120)]
        public int? DayPastDue { get; set; }

        [ColumnWidth(120)]
        public decimal? OverdueAmount { get; set; }

        [ColumnWidth(120)]
        public decimal? OutstandingBalance { get; set; }

        public DateTime? AssignDate { get; set; }

        [ColumnWidth(120)]
        public string FollowupStatus { get; set; }

        public DateTime? LastFollowupDate { get; set; }
    }
}
