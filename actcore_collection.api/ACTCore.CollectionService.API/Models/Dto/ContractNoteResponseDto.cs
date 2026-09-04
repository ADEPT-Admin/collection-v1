namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ContractNoteResponseDto
    {
        public DateTime? FollowupDate { get; set; }
        public string FollowupAction { get; set; }
        public string FollowupResult { get; set; }
        public string ContractPersonFullName { get; set; }
        public string ContractPhoneNumber { get; set; }
        public DateTime? NextFollowupDate { get; set; }

        public DateTime? PromiseToPayDate { get; set; }

        public decimal? PromiseToPayAmount { get; set; }
        public string CollectionRemark { get; set; }
        public string UpdatedBy { get; set; }

    }
}
