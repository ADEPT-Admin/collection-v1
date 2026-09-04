using ACTCore.CollectionService.Domain.Entities.Masters;
using ACTCore.CollectionService.Domain.Entities.Securities;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Profiles;

[Table("EmployeeProfile")]
public partial class EmployeeProfile : VersionBaseModel
{
    [Key]
    [MaxLength(20)]
    public string EmployeeId { get; set; }

    [MaxLength(100)]
    public string UserName { get; set; }

    public int? PrefixId { get; set; }

    [MaxLength(250)]
    public string FirstName { get; set; }

    [MaxLength(250)]
    public string LastName { get; set; }

    [NotMapped]
    public string FullName => $"{Prefix?.PrefixName ?? ""} {FirstName} {LastName}";

    [MaxLength(250)]
    public string FirstNameEn { get; set; }

    [MaxLength(250)]
    public string LastNameEn { get; set; }

    [NotMapped]
    public string FullNameEn => $"{Prefix?.PrefixNameEn ?? ""} {FirstNameEn} {LastNameEn}";

    [MaxLength(50)]
    public string NickName { get; set; }

    [MaxLength(1)]
    public string Gender { get; set; }

    public DateTime? DateOfBirth { get; set; }

    [MaxLength(50)]
    public string NationalID { get; set; }

    [MaxLength(50)]
    public string Nationality { get; set; }

    [MaxLength(50)]
    public string Religion { get; set; }

    [MaxLength(250)]
    public string Email { get; set; }

    [MaxLength(50)]
    public string PhoneNo { get; set; }

    public string Branch { get; set; }

    [MaxLength(20)]
    public string SupervisorId { get; set; }

    [MaxLength(20)]
    public string WorkStatus { get; set; }

    public int? DepartmentId { get; set; }

    public int? PositionId { get; set; }

    public int? TeamId { get; set; }

    public DateTime? StartWorkingDate { get; set; }

    public DateTime? ProbationDate { get; set; }

    public DateTime? WorkEffectiveDate { get; set; }

    public DateTime? TerminationDate { get; set; }

    public bool IsActive { get; set; }

    public DateTime? LastSyncDate { get; set; }

    [ForeignKey(nameof(PrefixId))]
    public virtual Prefix Prefix { get; set; }

    // Self-referencing navigation property
    [ForeignKey(nameof(SupervisorId)), InverseProperty("Subordinates")]
    public virtual EmployeeProfile Supervisor { get; set; }

    [ForeignKey(nameof(DepartmentId))]
    public virtual Department Department { get; set; }

    [ForeignKey(nameof(PositionId))]
    public virtual Position Position { get; set; }

    [ForeignKey(nameof(TeamId))]
    public virtual Team Team { get; set; }

    [InverseProperty(nameof(Supervisor))]
    public virtual ICollection<EmployeeProfile> Subordinates { get; set; } = new List<EmployeeProfile>();
    public virtual ICollection<SysUser> SysUsers { get; set; } = new List<SysUser>();
}
