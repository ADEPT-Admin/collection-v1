using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColRoleRequestDto
    {

        [Required]
        public string ColRoleCode { get; set; }

        public string ColRoleName { get; set; }

        public string Description { get; set; }

        [Required]
        public bool IsActive { get; set; }
    }
}
