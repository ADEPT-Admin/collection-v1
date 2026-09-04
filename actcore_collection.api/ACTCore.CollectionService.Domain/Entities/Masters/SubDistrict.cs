using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("SubDistrict")]
public partial class SubDistrict : VersionBaseModel
{
    [Key]
    [MaxLength(20)]
    public string SubDistrictId { get; set; }

    [Required]
    public string DistrictId { get; set; }

    [Required, MaxLength(250)]
    public string SubDistrictName { get; set; }

    [MaxLength(250)]
    public string SubDistrictNameEn { get; set; }

    public float? Latitude { get; set; }

    public float? Longtitude { get; set; }

    [MaxLength(20)]
    public string ZipCode { get; set; }

    [ForeignKey(nameof(DistrictId))]
    public virtual District District { get; set; }
}
