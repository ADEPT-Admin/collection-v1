using ACTCore.CollectionService.Domain.ValueObjects;
using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysItemCreateDto
    {
        [Required]
        public string ItemId { get; set; }
        public string ParentId { get; set; }
        [Required]
        public LanguageValue ItemName { get; set; }
        public string RouteName { get; set; }
        public int? ItemOrder { get; set; }
        public int ItemLevel { get; set; }
        public string Icon { get; set; }
        public string ToolTip { get; set; }
        [Required]
        public bool IsActive { get; set; }
    }
}
