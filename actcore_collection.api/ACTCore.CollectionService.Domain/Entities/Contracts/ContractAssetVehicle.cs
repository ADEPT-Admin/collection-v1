using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractAssetVehicle")]
    public class ContractAssetVehicle : VersionBaseModel
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

        [MaxLength(250)]
        public string Brand { get; set; }

        [MaxLength(250)]
        public string Model { get; set; }

        [MaxLength(250)]
        public string Series { get; set; }

        public int? Year { get; set; }

        [MaxLength(250)]
        public string Color { get; set; }

        [MaxLength(100)]
        public string EngineNo { get; set; }

        [MaxLength(250)]
        public string ChassisNo { get; set; }

        [MaxLength(50)]
        public string PlateNo { get; set; }

        public string RegisterProvince { get; set; }

        [MaxLength(250)]
        public string FuelType { get; set; }
    }
}
