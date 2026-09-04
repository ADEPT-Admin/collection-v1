using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColTeamDeleteDto
    {
        [Required]
        public Guid ColTeamId { get; set; }
    }
}
