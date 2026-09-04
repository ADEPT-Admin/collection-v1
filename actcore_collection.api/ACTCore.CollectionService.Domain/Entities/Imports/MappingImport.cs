using SharedKernel.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACTCore.CollectionService.Domain.Entities.Imports
{
    [Table("MappingImport")]
    public class MappingImport: VersionBaseModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string TableName { get; set; } = null!;

        [MaxLength(50)]
        public string NestedTable { get; set; }

        [Required]
        public bool AllowImport { get; set; }

        public ICollection<MappingImportDetail> MappingImportDetails { get; set; }
            = new List<MappingImportDetail>();

    }
}
