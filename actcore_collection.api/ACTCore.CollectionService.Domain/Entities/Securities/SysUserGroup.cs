using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysUserGroup")]
public partial class SysUserGroup : VersionBaseModel
{
    [Key]
    public Guid UserGroupId { get; set; } = new Guid();

    [Required, MaxLength(50)]
    public string UserGroupCode { get; set; }

    [Required, MaxLength(250)]
    public string UserGroupName { get; set; }

    [MaxLength(500)]
    public string Description { get; set; }

    public bool IsActive { get; set; }

    public virtual ICollection<SysUserGroupAccessRight> UserGroupAccesses { get; set; } = new List<SysUserGroupAccessRight>();

    public virtual ICollection<SysItemAccessRight> ItemAccessRights { get; set; } = new List<SysItemAccessRight>();
}
