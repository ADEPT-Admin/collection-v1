using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysUser")]
public partial class SysUser : VersionBaseModel
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();

    [Required, MaxLength(100)]
    public string UserName { get; set; } = null!;

    [MaxLength(20)]
    public string EmployeeId { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public bool IsActive { get; set; }

    public bool IsLockUser { get; set; }

    public bool IsAdmin { get; set; }

    public bool IsNewUser { get; set; }

    public DateTime? LastSignOnDate { get; set; }

    public DateTime? LastChangePasswordDate { get; set; }

    public int? MissingHit { get; set; }

    public DateTime? EffectiveDate { get; set; }

    public DateTime? ExpireDate { get; set; }

    [ForeignKey(nameof(EmployeeId))]
    public virtual EmployeeProfile Employee { get; set; }

    public virtual ICollection<ActiveSession> ActiveSessions { get; set; }
    public virtual ICollection<SysUserGroupAccessRight> UserGroupAccesses { get; set; } = new List<SysUserGroupAccessRight>();
    public virtual ICollection<CollectorProfile> Collectors { get; set; } = new List<CollectorProfile>();

}
