using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColNoteResult")]
    public class ColNoteResult : VersionBaseModel
    {
        [Key]
        public int ResultId { get; set; }

        [Required]
        public int ActionId { get; set; }

        [ForeignKey(nameof(ActionId))]
        public virtual ColNoteAction ColNoteAction { get; set; }

        [Required]
        [MaxLength(20)]
        public string ResultCode { get; set; }

        [Required]
        [MaxLength(250)]
        public string ResultDescription { get; set; }

        [Required]
        [MaxLength(50)]
        public string FollowupStatus { get; set; }

        public bool IsActive { get; set; }
    }
}
