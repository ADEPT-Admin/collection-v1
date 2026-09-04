using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorDeleteDto
    {
        [Required]
        public Guid CollectorId { get; set; }
    }
}
