using SharedKernel.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[PrimaryKey("DocumentName", "DocumentPrefix", "DocumentCode")]
[Table("SysRunningNo")]
public partial class SysRunningNo : VersionBaseModel
{
    [Key]
    [MaxLength(50)]
    public string DocumentName { get; set; }

    [Key]
    [MaxLength(50)]
    public string DocumentCode { get; set; }

    [Key]
    [MaxLength(50)]
    public string DocumentPrefix { get; set; }

    [Required]
    [MaxLength(50)]
    public string DocumentFormat { get; set; }

    public int RunningNo { get; set; }
}
