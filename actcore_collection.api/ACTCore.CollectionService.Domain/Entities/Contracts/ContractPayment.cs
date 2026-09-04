using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractPayment")]
    public class ContractPayment : VersionBaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(20)]
        [Required]
        [UpdateKey(1)]
        public string ContractNo { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(ContractNo))]
        public virtual Contract Contract { get; set; }
        
        [UpdateKey(2)]
        public DateTime? PaymentDate { get; set; }

        public decimal? PaymentAmount { get; set; }

        public decimal? InstallAmount { get; set; }

        public decimal? PenaltyAmount { get; set; }

        public decimal? OtherFeesAmount { get; set; }

        [MaxLength(250)]
        public string PaymentChannel { get; set; }

        [MaxLength(250)]
        public string ReferenceNo { get; set; }
    }
}
