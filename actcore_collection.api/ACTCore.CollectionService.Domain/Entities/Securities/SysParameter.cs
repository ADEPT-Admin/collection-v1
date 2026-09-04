using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Securities
{
    [Table("SysParameter")]
    public class SysParameter : VersionBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        public string ParameterCategory { get; set; }

        [Required, MaxLength(100)]
        public string ParameterName { get; set; }

        [Required]
        [MaxLength(250)]
        public string ParameterValue { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        public bool IsSystem { get; set; }
    }
}