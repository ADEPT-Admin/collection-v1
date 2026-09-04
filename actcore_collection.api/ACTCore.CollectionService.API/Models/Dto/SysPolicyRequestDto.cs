using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysPolicyRequestDto
    {
        [Required]
        public int Id { get; set; }
        
        public string PolicyValue { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
