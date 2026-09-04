using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Department")]
public partial class Department : VersionBaseModel
{
    [Key]
    public int DepartmentId { get; set; }

    [Required, MaxLength(20)]
    public string DepartmentCode { get; set; }

    [Required, MaxLength(250)]
    public string DepartmentName { get; set; }

    [MaxLength(250)]
    public string DepartmentNameEn { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public virtual ICollection<EmployeeProfile> Employees { get; set; } = new List<EmployeeProfile>();

    //public virtual ICollection<Position> Positions { get; set; } = new List<Position>();
}
