using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Position")]
public partial class Position : VersionBaseModel
{
    [Key]
    public int PositionId { get; set; }

    [Required, MaxLength(20)]
    public string PositionCode { get; set; }

    [Required, MaxLength(250)]
    public string PositionName { get; set; }

    [MaxLength(250)]
    public string PositionNameEn { get; set; }

    public int? Level { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeProfile> Employees { get; set; } = new List<EmployeeProfile>();
}
