namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class WorklistDisplayApproveListingFilterDto
    {
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public string AssetType { get; set; }
        public string ContractStatus { get; set; }
        public string FollowupStatus { get; set; }
        public string ReassignFrom { get; set; }
        public string TeamFrom { get; set; }
        public string ReassignTo { get; set; }
        public string RequestedDate { get; set; }
        public string Reason { get; set; }
    }
}
