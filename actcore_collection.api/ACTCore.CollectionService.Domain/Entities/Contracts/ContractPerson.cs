using ACTCore.CollectionService.Domain.Attributes;
using ACTCore.CollectionService.Domain.Entities.Masters;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("ContractPerson")]
    public class ContractPerson : VersionBaseModel
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(50)]
        [Required]
        [UpdateKey(1)]
        public string PersonRefId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ContractNo { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(ContractNo))]
        public virtual Contract Contract { get; set; }

        [MaxLength(20)]
        public string PersonType { get; set; }

        public int? PrefixId { get; set; }

        [ForeignKey(nameof(PrefixId))]
        public virtual Prefix Prefix { get; set; }

        [MaxLength(250)]
        public string FirstName { get; set; }

        [MaxLength(250)]
        public string LastName { get; set; }

        [NotMapped]
        public string FullName => $"{Prefix.PrefixName} {FirstName} {LastName}";

        [MaxLength(50)]
        public string IdCard { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? Age { get; set; }

        [MaxLength(20)]
        public string Gender { get; set; }

        [MaxLength(50)]
        public string MaritalStatus { get; set; }

        [MaxLength(250)]
        public string Occupation { get; set; }

        public decimal? Income { get; set; }

        [MaxLength(250)]
        public string WorkPlace { get; set; }

        [MaxLength(250)]
        public string Email { get; set; }

        [MaxLength(250)]
        public string Relationship { get; set; }

        public virtual ICollection<ContractAddress> ContractAddresses { get; set; } = new List<ContractAddress>();
        public virtual ICollection<ContractPhone> ContractPhones { get; set; } = new List<ContractPhone>();
    }
}
