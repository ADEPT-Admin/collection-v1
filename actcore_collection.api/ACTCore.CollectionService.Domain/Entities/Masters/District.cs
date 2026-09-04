using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("District")]
public partial class District : VersionBaseModel
{

    [Key]
    [MaxLength(20)]
    public string DistrictId { get; set; }

    [Required, MaxLength(20)]
    public string ProvinceId { get; set; }

    [Required, MaxLength(250)]
    public string DistrictName { get; set; }

    [MaxLength(250)]
    public string DistrictNameEn { get; set; }

    [ForeignKey(nameof(ProvinceId))]
    public virtual Province Province { get; set; }

    public virtual ICollection<SubDistrict> SubDistricts { get; set; } = new List<SubDistrict>();
}
