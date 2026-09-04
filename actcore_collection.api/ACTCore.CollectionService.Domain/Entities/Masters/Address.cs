using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Masters;

[Table("Address")]
public partial class Address : VersionBaseModel
{
    [Key]
    public Guid AddressId { get; set; }

    public string AddressLine1 { get; set; }

    public string AddressLine2 { get; set; }

    public int? SubDistrictId { get; set; }

    public int? DistrictId { get; set; }

    public int? ProvinceId { get; set; }

    [MaxLength(20)]
    public string ZipCode { get; set; }

    public virtual ICollection<EmployeeProfile> EmployeeCurrentAddresses { get; set; } = new List<EmployeeProfile>();

    public virtual ICollection<EmployeeProfile> EmployeeRegistrationAddresses { get; set; } = new List<EmployeeProfile>();
}
