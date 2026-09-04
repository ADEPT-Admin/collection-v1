using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractAddress")]
    public class ContractAddress : VersionBaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        [MaxLength(50)]
        [UpdateKey(1)]
        public string PersonRefId { get; set; }

        [ForeignKey(nameof(PersonRefId))]
        public virtual ContractPerson ContractPerson { get; set; }

        [MaxLength(50)]
        [UpdateKey(2)]
        public string AddressType { get; set; }

        [MaxLength(500)]
        public string Address { get; set; }

        [MaxLength(100)]
        public string Province { get; set; }

        [MaxLength(100)]
        public string District { get; set; }

        [MaxLength(100)]
        public string SubDistrict { get; set; }

        [MaxLength(5)]
        public string ZipCode { get; set; }

        [MaxLength(500)]
        public string AddressRemark { get; set; }
    }
}
