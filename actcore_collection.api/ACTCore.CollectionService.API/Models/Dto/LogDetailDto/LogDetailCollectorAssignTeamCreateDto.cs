
namespace ACTCore.CollectionService.API.Models.Dto
{
    public class LogDetailCollectorAssignTeamCreateDto
    {
        public Guid ColTeamId { get; set; }

        public Guid CollectorId { get; set; }

        public int Capacity { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public bool IsActive { get; set; }
    }
}
