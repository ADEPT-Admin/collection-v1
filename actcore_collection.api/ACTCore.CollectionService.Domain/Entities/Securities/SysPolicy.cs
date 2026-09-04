using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysPolicy")]

public partial class SysPolicy : VersionBaseModel
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string PolicyCategory { get; set; }

    [Required, MaxLength(20)]
    public string PolicyCode { get; set; }

    [Required, MaxLength(100)]
    public string PolicyName { get; set; }

    [MaxLength(100)]
    public string PolicyValue { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    [Required]
    public bool IsActive { get; set; } = true;

}

