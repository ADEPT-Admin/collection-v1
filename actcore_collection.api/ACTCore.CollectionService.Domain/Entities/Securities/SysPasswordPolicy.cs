using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysPasswordPolicy")]
public partial class SysPasswordPolicy : VersionBaseModel
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [MaxLength(10)]
    public string PolicyCode { get; set; }

    [MaxLength(250)]
    public string PolicyName { get; set; }

    public string PolicyValue { get; set; }

    public bool IsActive { get; set; }
}
