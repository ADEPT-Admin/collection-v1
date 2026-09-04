using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysItemIdRequestDto
    {
        [Required]
        public string ItemId { get; set; }
    }
}
