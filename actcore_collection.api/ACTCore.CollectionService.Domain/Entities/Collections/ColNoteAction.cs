using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColNoteAction")]
    public class ColNoteAction : VersionBaseModel
    {
        [Key]
        public int ActionId { get; set; }

        [Required]
        [MaxLength(20)]
        public string ActionCode { get; set; }

        [Required]
        [MaxLength(250)]
        public string ActionDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
