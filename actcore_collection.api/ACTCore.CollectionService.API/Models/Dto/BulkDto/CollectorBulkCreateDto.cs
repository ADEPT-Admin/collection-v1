using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class CollectorBulkCreateDto
    {
        [Required]
        public Guid ColRoleId { get; set; }
        
        public bool IsActive { get; set; }

        [Required]
        public List<Guid> UserIds { get; set; }

    }
}