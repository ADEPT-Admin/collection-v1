using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColTeamCreateDto
    {
        [Required]
        public string ColTeamCode { get; set; }

        [Required]
        public string ColTeamName { get; set; }

        public int Capacity { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
