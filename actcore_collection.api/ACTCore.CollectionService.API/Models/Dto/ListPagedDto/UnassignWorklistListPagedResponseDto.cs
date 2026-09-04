namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class UnassignWorklistListPagedResponseDto
    {
        public Guid WorklistId { get; set; }
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public string AssetType { get; set; }
        public string ContractStatus { get; set; }
        public int? Bucket { get; set; }
        public int? PaymentDueDate { get; set; }
        public int? DayPastDue { get; set; }
        public decimal? OverdueAmount { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public DateTime? AssignDate { get; set; }
        public string FollowupStatus { get; set; }
        public DateTime? LastFollowupDate { get; set; }
    }
}
