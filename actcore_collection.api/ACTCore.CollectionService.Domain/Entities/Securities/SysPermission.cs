using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("SysPermission")]
    public class SysPermission : VersionBaseModel
    {
        [Key]
        public Guid PermissionId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public string PermissionCode { get; set; } = null!;

        [Required, MaxLength(250)]
        public string PermissionName { get; set; } = null!;

        [MaxLength(100)]
        public string? Resource { get; set; }

        [MaxLength(100)]
        public string? Action { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public virtual ICollection<SysRolePermission> RolePermissions { get; set; } = new List<SysRolePermission>();
    }

}
