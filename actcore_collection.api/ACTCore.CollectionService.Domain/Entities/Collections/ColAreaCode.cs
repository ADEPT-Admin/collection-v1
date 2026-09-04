using ACTCore.CollectionService.Domain.Entities.Masters;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("ColArea")]
    public class ColArea : VersionBaseModel
    {
        [Key]
        public Guid AreaId { get; set; }

        public Guid? ParentId { get; set; }

        [MaxLength(20)]
        public string AreaCode { get; set; }

        [MaxLength(250)]
        public string AreaName { get; set; }

        public string AreaLevelId { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(AreaLevelId))]
        public virtual Province AreaLevel { get; set; }

        public string ProvinceId { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(ProvinceId))]
        public virtual Province Province { get; set; }

        public string DistrictId { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(DistrictId))]
        public virtual District District { get; set; }

        public string SubDistrictId { get; set; }

        // Navigation property (reference)
        [ForeignKey(nameof(SubDistrictId))]
        public virtual SubDistrict SubDistrict { get; set; }

        public bool IsActive { get; set; }

        public ICollection<ColTeam> ColTeams { get; set; } = new List<ColTeam>();
    }
}
