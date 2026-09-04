using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorCreateDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid ColRoleId { get; set; }

        public bool IsActive { get; set; }
    }
}
