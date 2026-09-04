using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("SysRole")]
    public class SysRole : VersionBaseModel
    {
        [Key]
        public Guid RoleId { get; set; } = Guid.NewGuid();

        [Required, MaxLength(50)]
        public string RoleCode { get; set; } = null!;

        [Required, MaxLength(250)]
        public string RoleName { get; set; } = null!;

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsSystem { get; set; } = false;

        public bool IsActive { get; set; } = true;

        public virtual ICollection<SysUserGroupRole> GroupRoles { get; set; } = new List<SysUserGroupRole>();

        public virtual ICollection<SysRolePermission> RolePermissions { get; set; } = new List<SysRolePermission>();
    }

}
