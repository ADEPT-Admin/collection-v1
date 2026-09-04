using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class TeamAssignmentDeleteDto
    {
        [Required]
        public Guid AssignmentId { get; set; }
    }
}
