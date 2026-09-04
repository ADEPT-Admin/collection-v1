using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class UserRequestDto
    {
        [Required]
        public Guid UserId { get; set; }
    }
}