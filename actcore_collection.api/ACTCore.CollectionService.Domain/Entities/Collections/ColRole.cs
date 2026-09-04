using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColRole")]
    public class ColRole : VersionBaseModel
    {
        [Key]
        public Guid ColRoleId { get; set; }

        [Required, MaxLength(50)]
        public string ColRoleCode { get; set; }

        [Required, MaxLength(250)]
        public string ColRoleName { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ColRolePermission> ColRolePermissions { get; set; } = new List<ColRolePermission>();
        public virtual ICollection<CollectorProfile> Collectors { get; set; } = new List<CollectorProfile>();
    }
}
