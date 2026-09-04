using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [PrimaryKey(nameof(UserGroupId), nameof(RoleId))]
    [Table("SysUserGroupRole")]
    public class SysUserGroupRole : VersionBaseModel
    {
        [Key]
        public Guid UserGroupId { get; set; }

        [Key]
        public Guid RoleId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(UserGroupId))]
        public virtual SysUserGroup UserGroup { get; set; } = null!;

        [ForeignKey(nameof(RoleId))]
        public virtual SysRole Role { get; set; } = null!;
    }

}
