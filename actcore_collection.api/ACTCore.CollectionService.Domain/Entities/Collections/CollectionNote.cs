using ACTCore.CollectionService.Domain.Entities.Contracts;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("CollectionNote")]
    public class CollectionNote : VersionBaseModel
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(20)]
        public Guid WorklistId { get; set; }

        [ForeignKey(nameof(WorklistId))]
        public virtual Worklist Worklist { get; set; }

        [MaxLength(20)]
        public string ContractNo { get; set; }

        [Required]
        public DateTime FollowupDate { get; set; } = DateTime.Now;

        public int? FollowupActionId { get; set; }

        [ForeignKey(nameof(FollowupActionId))]
        public virtual ColNoteAction ColNoteAction { get; set; }

        public int? FollowupResultId { get; set; }

        [ForeignKey(nameof(FollowupResultId))]
        public virtual ColNoteResult ColNoteResult { get; set; }

        public Guid ContractPersonId { get; set; }
        
        [ForeignKey(nameof(ContractPersonId))]
        public virtual ContractPerson ContractPerson { get; set; }

        [MaxLength(100)]
        public string ContractPersonFullName { get; set; }

        public Guid ContractPhoneId { get; set; }

        [ForeignKey(nameof(ContractPhoneId))]
        public virtual ContractPhone ContractPhone { get; set; }

        [MaxLength(50)]
        public string ContractPhoneNumber { get; set; }

        public DateTime? NextFollowupDate { get; set; }

        public DateTime? PromiseToPayDate { get; set; }

        public decimal? PromiseToPayAmount { get; set; }

        [MaxLength(500)]
        public string CollectionRemark { get; set; }
    }
}
