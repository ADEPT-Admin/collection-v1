using ACTCore.CollectionService.Domain.Attributes;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractPhone")]
    public class ContractPhone : VersionBaseModel
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
        public string PhoneType { get; set; }

        [MaxLength(50)]
        public string PhoneNo { get; set; }

        [MaxLength(500)]
        public string PhoneRemark { get; set; }
    }
}
