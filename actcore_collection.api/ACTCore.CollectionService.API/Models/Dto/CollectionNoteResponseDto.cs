namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectionNoteResponseDto
    {
        public Guid Id { get; set; }
        public string WorklistId { get; set; }
        public string ContractNo { get; set; }
        public DateTime? FollowupDate { get; set; }
        public string FollowupActionId { get; set; }
        public string FollowupResultId { get; set; }
        public string ContractPersonFullName { get; set; }
        public string ContractPhoneNumber { get; set; }
        public DateTime? NextFollowupDate { get; set; }
        public DateTime? PromiseToPayDate { get; set; }
        public decimal? PromiseToPayAmount { get; set; }
        public string CollectionRemark { get; set; }
    }
}
