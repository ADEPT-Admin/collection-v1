using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("SysEnum")]
    public class SysEnum : VersionBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(50)]
        public string EnumName { get; set; }

        [Required, MaxLength(50)]
        public string EnumCode { get; set; }

        [Required, MaxLength(250)]
        public string EnumDescription { get; set; }

        [MaxLength(250)]
        public string EnumDescriptionEn { get; set; }

        public int? EnumOrder { get; set; }

        [MaxLength(500)]
        public string Note { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;
    }
}
