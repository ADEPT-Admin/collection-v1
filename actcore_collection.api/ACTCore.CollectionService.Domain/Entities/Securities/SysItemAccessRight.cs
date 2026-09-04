using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[PrimaryKey(nameof(UserGroupId), nameof(ItemId))]
[Table("SysItemAccessRight")]
public partial class SysItemAccessRight : VersionBaseModel
{
    [Key]
    public Guid UserGroupId { get; set; }

    [Key, MaxLength(20)]
    public string ItemId { get; set; }

    [Required]
    public bool AllowAccess { get; set; }

    [Required]
    public bool AllowView { get; set; }

    [Required]
    public bool AllowNew { get; set; }

    [Required]
    public bool AllowEdit { get; set; }

    [Required]
    public bool AllowDelete { get; set; }

    [ForeignKey(nameof(ItemId))]
    public virtual SysItem Item { get; set; }

    [ForeignKey(nameof(UserGroupId))]
    public virtual SysUserGroup UserGroup { get; set; }
}
