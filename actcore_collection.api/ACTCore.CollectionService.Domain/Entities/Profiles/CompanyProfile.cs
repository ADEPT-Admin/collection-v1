using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Profiles;

[Table("CompanyProfile")]
public partial class CompanyProfile : VersionBaseModel
{
    [Key]
    [Column("CompanyID")]
    public int CompanyId { get; set; }

    [MaxLength(5)]
    public string CompanyCode { get; set; }

    [MaxLength(100)]
    public string CompanyName { get; set; }

    [Column("CompanyNameEN")]
    [MaxLength(100)]
    public string CompanyNameEn { get; set; }

    [MaxLength(100)]
    public string ContractName { get; set; }

    public DateTime? StartDate { get; set; }

    [MaxLength(100)]
    public string WorkEmail { get; set; }

    [MaxLength(100)]
    public string PesonalEmail { get; set; }

    [MaxLength(250)]
    public string PhoneNo { get; set; }

    [MaxLength(250)]
    public string MobileNo { get; set; }

    [MaxLength(250)]
    public string FaxNo { get; set; }

    [Column("TaxID")]
    [MaxLength(20)]
    public string TaxId { get; set; }

    public bool IsActive { get; set; }

}
