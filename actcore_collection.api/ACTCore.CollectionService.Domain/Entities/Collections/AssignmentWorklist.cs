using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("AssignmentWorklist")]
    public class AssignmentWorklist : VersionBaseModel
    {
        [Key]
        public Guid AssignmentWorklistId { get; set; }
                
        public int ADEPTProcessingDate { get; set; }

        public int ADEPTProcessingTime { get; set; }
        
        [MaxLength(100)]
        public string ADEPTProcessingVersion { get; set; }

        [Required]
        [MaxLength(50)]
        public string ContractNo { get; set; }

        [MaxLength(50)]
        public string AllocateMode { get; set; }

        [MaxLength(50)]
        public string AssignType { get; set; }

        [MaxLength(50)]
        public string DoNotCallFlag { get; set; }

        [MaxLength(50)]
        public string AssignFlag { get; set; }

        [MaxLength(500)]
        public string AssignReason { get; set; }

        [MaxLength(50)]
        public string AssignMethod { get; set; }

        [MaxLength(500)]
        public string AssignMethodSortField { get; set; }

        [MaxLength(500)]
        public string AssignMethodSortOrder { get; set; }

        [MaxLength(50)]
        public string AssignTool { get; set; }

        [MaxLength(50)]
        public string AssignTeam { get; set; }

        [MaxLength(50)]
        public string AssignAreaCode { get; set; }

        [MaxLength(50)]
        public string ProvinceCode { get; set; }

        [MaxLength(50)]
        public string DistrictCode { get; set; }

        [MaxLength(50)]
        public string SubDistrictCode { get; set; }

        [MaxLength(50)]
        public string AssignOverCapacity { get; set; }

        [MaxLength(50)]
        public string JobType { get; set; }

        [MaxLength(50)]
        public string AssignCollectorId { get; set; }

        public bool IsAssigned { get; set; }


        //[MaxLength(50)]
        //public string SupervisorId { get; set; }

        //[MaxLength(50)]
        //public string CreatedBy { get; set; }

        //public DateTime CreatedDate { get; set; }

        //[MaxLength(50)]
        //public string UpdatedBy { get; set; }

        //public DateTime UpdatedDate { get; set; }       


    }
}
