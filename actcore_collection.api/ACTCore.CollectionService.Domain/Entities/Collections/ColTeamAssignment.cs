using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColTeamAssignment")]
    public class ColTeamAssignment : VersionBaseModel
    {
        [Key]
        public Guid AssignmentId { get; set; }

        [Required]
        public Guid ColTeamId { get; set; }

        [Required]
        public Guid CollectorId { get; set; }

        [Required]
        public bool IsSupervisor { get; set; }

        [Required]
        public int Capacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required]
        public bool IsActive { get; set; }

        // 🔗 Navigation Properties
        [ForeignKey(nameof(ColTeamId))]
        public virtual ColTeam ColTeam { get; set; }

        [ForeignKey(nameof(CollectorId))]
        public virtual CollectorProfile Collector { get; set; }
    }
}
