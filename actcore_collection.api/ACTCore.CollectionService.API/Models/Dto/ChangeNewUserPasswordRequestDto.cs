using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ChangeNewUserPasswordRequestDto
    {
        [Required]
        public string NewPassword { get; set; }
        [Required]
        public string ConfirmPassword { get; set; }
    }
}
