using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [PrimaryKey(nameof(RoleId), nameof(PermissionId))]
    [Table("SysRolePermission")]
    public class SysRolePermission : VersionBaseModel
    {
        [Key]
        public Guid RoleId { get; set; }
        [Key]
        public Guid PermissionId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(RoleId))]
        public virtual SysRole Role { get; set; } = null!;

        [ForeignKey(nameof(PermissionId))]
        public virtual SysPermission Permission { get; set; } = null!;
    }

}
