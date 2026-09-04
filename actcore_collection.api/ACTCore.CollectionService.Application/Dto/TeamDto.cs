using ACTCore.CollectionService.Domain.Entities.Profiles;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Application.Dto
{
    public class TeamDto
    {
        public int TeamId { get; set; }

        public string TeamCode { get; set; }

        public string TeamName { get; set; }

        public bool IsActive { get; set; }

        public List<EmployeeProfile> EmployeeProfiles { get; set; }
    }
}
