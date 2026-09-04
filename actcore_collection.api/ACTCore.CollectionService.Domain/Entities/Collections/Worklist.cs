using ACTCore.CollectionService.Domain.Entities.Contracts;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("Worklist")]
    public class Worklist : VersionBaseModel
    {
        [Key]
        public Guid WorklistId { get; set; }

        public DateTime? AssignDate { get; set; }

        [Required, MaxLength(20)]
        public string ContractNo { get; set; }

        [ForeignKey(nameof(ContractNo))]
        public virtual Contract Contract { get; set; }

        [MaxLength(50)]
        public string JobTypeCode { get; set; }

        [MaxLength(250)]
        public string JobTypeDesc { get; set; }

        [MaxLength(50)]
        public string RiskLevel { get; set; }

        [MaxLength(50)]
        public string AssignTool { get; set; }

        public Guid? AssignAreaId { get; set; }

        [ForeignKey(nameof(AssignAreaId))]
        public virtual ColArea Area { get; set; }

        [MaxLength(20)]
        public string AssignAreaCode { get; set; }

        public Guid? AssignTeamId { get; set; }

        [MaxLength(20)]
        public string AssignTeamCode { get; set; }

        [MaxLength(250)]
        public string AssignTeamName { get; set; }

        [ForeignKey(nameof(AssignTeamId))]
        public virtual ColTeam CollectionTeam { get; set; }

        public Guid? AssignCollectorId { get; set; }

        [MaxLength(20)]
        public string AssignCollectorEmpId { get; set; }

        [MaxLength(250)]
        public string AssignCollectorName { get; set; }

        [ForeignKey(nameof(AssignCollectorId))]
        public virtual CollectorProfile Collector { get; set; }

        [MaxLength(50)]
        public string AssignTypeCode { get; set; }

        [MaxLength(250)]
        public string AssignTypeDesc { get; set; }

        public Guid? ReassignById { get; set; }

        [MaxLength(20)]
        public string ReassignByEmpId { get; set; }

        [MaxLength(250)]
        public string ReassignByName { get; set; }

        public DateTime? ReassignRequestDate { get; set; }

        public Guid? ReassignFromId { get; set; }

        [MaxLength(20)]
        public string ReassignFromEmpId { get; set; }

        [MaxLength(250)]
        public string ReassignFromName { get; set; }

        [ForeignKey(nameof(ReassignFromId))]
        public virtual CollectorProfile ReassignFrom { get; set; }

        public Guid? ReassignToId { get; set; }

        [MaxLength(20)]
        public string ReassignToEmpId { get; set; }

        [MaxLength(250)]
        public string ReassignToName { get; set; }


        [ForeignKey(nameof(ReassignToId))]
        public virtual CollectorProfile ReassignTo { get; set; }

        public Guid? ReassignToTeamId { get; set; }

        [MaxLength(20)]
        public string ReassignToTeamCode { get; set; }

        [MaxLength(250)]
        public string ReassignToTeamName { get; set; }

        [ForeignKey(nameof(ReassignToTeamId))]
        public virtual ColTeam ReassignToTeam { get; set; }

        [MaxLength(250)]
        public string ReassignReason { get; set; }

        [MaxLength(50)]
        public string ReassignStatusCode { get; set; }

        [MaxLength(250)]
        public string ReassignStatusDesc { get; set; }

        public Guid? ApprovedById { get; set; }

        [MaxLength(20)]
        public string ApprovedEmpId { get; set; }

        [MaxLength(250)]
        public string ApprovedName { get; set; }

        [MaxLength(500)]
        public string ApproveReason { get; set; }

        public DateTime? ApproveDate { get; set; }

        [MaxLength(50)]
        public string FollowupStatusCode { get; set; }

        [MaxLength(250)]
        public string FollowupStatusDesc { get; set; }

        public DateTime? FollowupDate { get; set; }

        [ForeignKey(nameof(ApprovedById))]
        public virtual CollectorProfile ApprovedBy { get; set; }

        public bool? DoNotCallFlag { get; set; } = false;

        // Navigation property (collection)
        public ICollection<CollectionNote> CollectionNotes { get; set; } = new List<CollectionNote>();
    }
}
