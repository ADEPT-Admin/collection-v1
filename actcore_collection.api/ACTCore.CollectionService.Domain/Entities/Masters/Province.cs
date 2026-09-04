using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Province")]
public partial class Province : VersionBaseModel
{
    [Key]
    [MaxLength(20)]
    public string ProvinceId { get; set; }

    [Required, MaxLength(250)]
    public string ProvinceName { get; set; }

    [MaxLength(50)]
    public string ProvinceAbbr { get; set; }

    [MaxLength(250)]
    public string ProvinceNameEn { get; set; }

    [MaxLength(50)]
    public string ProvinceAbbrEn { get; set; }

    [MaxLength(50)]
    public string RegionCode { get; set; }

    // Navigation property (collections)
    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
