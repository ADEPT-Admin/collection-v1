using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectionNoteCreateDto
    {
        [Required]
        public Guid WorklistId { get; set; }

        [Required]
        public string ContractNo { get; set; }

        [Required]
        public int FollowupActionId { get; set; }

        [Required]
        public int FollowupResultId { get; set; }

        [Required]
        public Guid ContractPersonId { get; set; }

        [Required]
        public Guid ContractPhoneId { get; set; }

        public DateTime? NextFollowupDate { get; set; }

        public DateTime? PromiseToPayDate { get; set; }

        public decimal? PromiseToPayAmount { get; set; }

        public string CollectionRemark { get; set; }
    }
}
