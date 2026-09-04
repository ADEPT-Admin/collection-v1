using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class TokenRequestDto
    {
        [Required]
        public string AccessToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
