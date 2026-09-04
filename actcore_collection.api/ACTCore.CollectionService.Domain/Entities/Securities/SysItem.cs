using ACTCore.CollectionService.Domain.ValueObjects;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysItem")]
public partial class SysItem : VersionBaseModel
{
    [Key]
    [MaxLength(20)]
    public string ItemId { get; set; }

    [MaxLength(20)]
    public string ParentId { get; set; }

    [Required]
    public string ItemName { get; set; }

    [MaxLength(250)]
    public string RouteName { get; set; }

    public int ItemLevel { get; set; }

    public int? ItemOrder { get; set; }

    [MaxLength(20)]
    public string Icon { get; set; }

    [MaxLength(250)]
    public string ToolTip { get; set; }

    [Required]
    public bool IsActive { get; set; }

    public virtual ICollection<SysItemAccessRight> ItemAccessRights { get; set; } = new List<SysItemAccessRight>();
    public virtual ICollection<SysUserItemFavorite> UserItemFavorites { get; set; } = new List<SysUserItemFavorite>();

    /* Computed Column */
    public string ItemNameEn { get; private set; } = null!;
    public string ItemNameTh { get; private set; } = null!;
}
