using ACTCore.CollectionService.Domain.Entities.Contracts;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Prefix")]
public partial class Prefix : VersionBaseModel
{
    [Key]
    public int PrefixId { get; set; }

    [Required, MaxLength(20)]
    public string PrefixCode { get; set; }

    [Required, MaxLength(250)]
    public string PrefixName { get; set; }

    [MaxLength(250)]
    public string PrefixNameEn { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeProfile> EmployeeProfiles { get; set; }
    public virtual ICollection<ContractPerson> ContractPersons { get; set; }
}
