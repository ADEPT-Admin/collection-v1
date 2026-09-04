using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractOverdue")]
    public class ContractOverdue : VersionBaseModel
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

        public int? Order { get; set; }

        [MaxLength(250)]
        [UpdateKey(2)]
        public string OverdueType { get; set; }

        public decimal? OverdueAmount { get; set; }
    }
}
