using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColRoleUpdateDto
    {
        [Required]
        public Guid ColRoleId { get; set; }

        [Required]
        public string ColRoleCode { get; set; }

        public string ColRoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
