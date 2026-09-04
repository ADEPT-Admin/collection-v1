namespace ACTCore.CollectionService.Application.Dto
{
    public class WorklistListringDto
    {
        public Guid WorklistId { get; set; }
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public string AssetType { get; set; }
        public string ContractStatus { get; set; }
        public int? Bucket { get; set; }
        public DateTime? DueDate { get; set; }
        public int? DayPastDue { get; set; }
        public decimal? OverdueAmount { get; set; }
        public decimal? OutstandingBalance { get; set; }
        public string CollectorName { get; set; }
        public string ReassignFrom { get; set; }
        public string ReassignBy { get; set; }
        public DateTime? AssignDate { get; set; }
        public string FollowupStatus { get; set; }
        public DateTime? LastFollowupDate { get; set; }
        public DateTime? NextFollowupDate { get; set; }
        public DateTime? PromiseToPayDate { get; set; }

    }
}
