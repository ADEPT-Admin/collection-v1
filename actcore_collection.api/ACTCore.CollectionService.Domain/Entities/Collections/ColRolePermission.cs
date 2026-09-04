using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [PrimaryKey(nameof(ColRoleId), nameof(ColPermissionId))]
    [Table("ColRolePermission")]
    public class ColRolePermission : VersionBaseModel
    {
        [Key]
        public Guid ColRoleId { get; set; }

        [Key]
        public Guid ColPermissionId { get; set; }

        public bool IsActive { get; set; } = true;

        [ForeignKey(nameof(ColRoleId))]
        public virtual ColRole ColRole { get; set; } = null!;

        [ForeignKey(nameof(ColPermissionId))]
        public virtual ColPermission ColPermission { get; set; } = null!;
    }
}
