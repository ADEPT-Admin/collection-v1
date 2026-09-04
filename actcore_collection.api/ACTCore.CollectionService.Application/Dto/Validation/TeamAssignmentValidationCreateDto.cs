using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Application.Dto.Validation
{
    public class TeamAssignmentValidationCreateDto
    {
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
