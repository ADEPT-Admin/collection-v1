namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class ReassignWorklistListPagedResponseDto
    {
        public Guid WorklistId { get; set; }
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public string AssetType { get; set; }
        public string ContractStatus { get; set; }
        public string FollowupStatus { get; set; }
        public string ReassignBy { get; set; }
        public string TeamFrom { get; set; }
        public string ReassignTo { get; set; }
        public string TeamTo { get; set; }
        public DateTime RequestedDate { get; set; }
        public string Reason { get; set; }
        public string ReassignStatus { get; set; }
        public string IsMyTeam { get; set; }
    }
}
