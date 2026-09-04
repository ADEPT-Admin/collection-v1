using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorUpdateDto
    {
        [Required]
        public Guid CollectorId { get; set; }

        [Required]
        public Guid ColRoleId { get; set; }

        public bool IsActive { get; set; }
    }
}
