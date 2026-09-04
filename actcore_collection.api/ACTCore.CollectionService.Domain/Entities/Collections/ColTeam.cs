using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColTeam")]
    public class ColTeam : VersionBaseModel
    {
        [Key]
        public Guid ColTeamId { get; set; }

        [Required, MaxLength(20)]
        public string ColTeamCode { get; set; }

        [Required, MaxLength(250)]
        public string ColTeamName { get; set; }

        //[Required] 
        [MaxLength(250)]
        public string ColTeamType { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        public int Capacity { get; set; }

        public bool IsActive { get; set; }

        public virtual ICollection<ColTeamAssignment> ColTeamAssignments { get; set; } = new List<ColTeamAssignment>();

    }
}
