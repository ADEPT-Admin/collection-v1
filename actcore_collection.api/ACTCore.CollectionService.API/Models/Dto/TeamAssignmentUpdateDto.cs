using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class TeamAssignmentUpdateDto
    {
        [Required]
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
    }
}
