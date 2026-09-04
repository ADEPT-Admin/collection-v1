using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities;

[Table("SysPasswordHistory")]
public partial class SysPasswordHistory : VersionBaseModel
{
    [Key]
    public Guid Id { get; set; }

    public Guid UserId { get; set; }

    [MaxLength(100)]
    public string UserName { get; set; }

    public string PasswordHash { get; set; }
}
