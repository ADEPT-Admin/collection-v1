using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Team")]
public partial class Team : VersionBaseModel
{
    [Key]
    public int TeamId { get; set; }

    [Required, MaxLength(20)]
    public string TeamCode { get; set; }

    [Required, MaxLength(250)]
    public string TeamName { get; set; }

    [MaxLength(250)]
    public string TeamNameEn { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeProfile> EmployeeProfiles { get; set; } = new List<EmployeeProfile>();
}
