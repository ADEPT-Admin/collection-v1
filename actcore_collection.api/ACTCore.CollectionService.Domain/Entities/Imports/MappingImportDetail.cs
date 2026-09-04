using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Imports
{
    [Table("MappingImportDetail")]
    public class MappingImportDetail: VersionBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int MappingId { get; set; }

        [Required]
        [MaxLength(50)]
        public string FieldName { get; set; } = null!;

        [Required]
        [MaxLength(25)]
        public string DataType { get; set; } = null!;

        [Required]
        public bool IsRequired { get; set; }

        [Required]
        [MaxLength(50)]
        public string MappingField { get; set; } = null!;

        [Required]
        public bool IsActive { get; set; }

        // Navigation Property
        [ForeignKey(nameof(MappingId))]
        public MappingImport MappingImport { get; set; } = null!;
    }
}
