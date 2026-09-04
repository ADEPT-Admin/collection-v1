using ACTCore.CollectionService.Domain.Entities.Securities;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("CollectorProfile")]
    public class CollectorProfile : VersionBaseModel
    {
        [Key]
        [MaxLength(20)]
        public Guid CollectorId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public Guid ColRoleId { get; set; }

        public bool IsActive { get; set; }

        [ForeignKey(nameof(UserId))]
        public virtual SysUser User { get; set; }

        [ForeignKey(nameof(ColRoleId))]
        public virtual ColRole ColRole { get; set; }

        public virtual ICollection<ColTeamAssignment> ColTeamAssignments { get; set; } = new List<ColTeamAssignment>();

        //public virtual ICollection<Worklist> Worklist { get; set; } = new List<Worklist>();

    }
}
