using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColPermission")]
    public class ColPermission : VersionBaseModel
    {
        [Key]
        public Guid ColPermissionId { get; set; }

        [Required, MaxLength(50)]
        public string ColPermissionCode { get; set; }

        [Required, MaxLength(250)]
        public string ColPermissionName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ColRolePermission> ColRolePermissions { get; set; } = new List<ColRolePermission>();
    }
}
