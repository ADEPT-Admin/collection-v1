using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Application.Dto.Validation
{
    public class CollectorAssignTeamValidationCreateDto
    {
        [Required]
        public Guid ColTeamId { get; set; }

        [Required]
        public List<Guid> CollectorIds { get; set; }

        [Required]
        public int Capacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
