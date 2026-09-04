using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("Language")]
public partial class Language : VersionBaseModel
{
    [Key]
    public string Key { get; set; } // Primary Key

    [Required]
    public string Value { get; set; }

    [Required]
    public string DefaultValue { get; set; }


    public int? Ordering { get; set; }

    /* Computed Columns */
    public string ValueEn { get; private set; } = null!;
    public string ValueTh { get; private set; } = null!;
}
