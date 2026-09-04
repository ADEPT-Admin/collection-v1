using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractAsset")]
    public class ContractAsset : VersionBaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(20)]
        [UpdateKey(1)]
        public string ContractNo { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(ContractNo))]
        public virtual Contract Contract { get; set; }

        [MaxLength(250)]
        public string AssetGroup { get; set; }

        [MaxLength(250)]
        public string AssetType { get; set; }

        [MaxLength(500)]
        public string AssetDescription { get; set; }

        public decimal? AssetPrice { get; set; }
    }
}
