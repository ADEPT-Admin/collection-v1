
using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class TeamAssignmentFlagResponseDto
    {
        public Guid CollectorId { get; set; }

        public string ColTeamName { get; set; }

        public LanguageValue CollectorEmpName { get; set; }

        public bool IsSupervisor { get; set; }
    }
}
