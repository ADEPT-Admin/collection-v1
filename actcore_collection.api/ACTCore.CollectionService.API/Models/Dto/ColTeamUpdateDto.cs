using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColTeamUpdateDto
    {
        [Required]
        public Guid ColTeamId { get; set; }

        [Required]
        public string ColTeamCode { get; set; }

        [Required]
        public string ColTeamName { get; set; }

        [Required]
        public int Capacity { get; set; }

        public string Description { get; set; }

        [Required]
        public bool? IsActive { get; set; }
    }
}
