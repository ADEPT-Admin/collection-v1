using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[PrimaryKey("UserId", "ItemId")]
public partial class SysUserItemFavorite : VersionBaseModel
{
    [Key]
    public Guid UserId { get; set; }

    [Key]
    [MaxLength(20)]
    public string ItemId { get; set; }

    public int Seq { get; set; }

    [ForeignKey(nameof(ItemId))]
    public virtual SysItem Item { get; set; }

    [ForeignKey(nameof(UserId))]
    public virtual SysUser User { get; set; }
}
