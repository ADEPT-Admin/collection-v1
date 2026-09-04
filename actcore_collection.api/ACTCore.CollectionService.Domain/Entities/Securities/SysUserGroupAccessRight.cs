using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[PrimaryKey(nameof(UserId), nameof(UserGroupId))]
[Table("SysUserGroupAccessRight")]
public partial class SysUserGroupAccessRight : VersionBaseModel
{
    [Key]
    public Guid UserId { get; set; }

    [Key]
    public Guid UserGroupId { get; set; }

    public bool IsActive { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual SysUser User { get; set; }

    [ForeignKey(nameof(UserGroupId))]
    public virtual SysUserGroup UserGroup { get; set; }
}
