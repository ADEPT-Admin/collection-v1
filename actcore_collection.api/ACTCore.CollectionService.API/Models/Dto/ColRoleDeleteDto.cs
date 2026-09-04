using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColRoleDeleteDto
    {
        [Required]
        public Guid ColRoleId { get; set; }
    }
}
