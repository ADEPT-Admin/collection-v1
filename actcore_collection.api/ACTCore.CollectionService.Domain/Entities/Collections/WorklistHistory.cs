using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Collections
{
    [Table("WorklistHistory")]
    public class WorklistHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid WorklistId { get; set; }

        public DateTime? AssignDate { get; set; }

        [Required, MaxLength(20)]
        public string ContractNo { get; set; }

        [MaxLength(50)]
        public string JobTypeCode { get; set; }

        [MaxLength(250)]
        public string JobTypeDesc { get; set; }

        [MaxLength(50)]
        public string RiskLevel { get; set; }

        [MaxLength(50)]
        public string AssignTool { get; set; }

        public Guid? AssignAreaId { get; set; }

        [MaxLength(20)]
        public string AssignAreaCode { get; set; }

        public Guid? AssignTeamId { get; set; }

        [MaxLength(20)]
        public string AssignTeamCode { get; set; }

        [MaxLength(250)]
        public string AssignTeamName { get; set; }

        public Guid? AssignCollectorId { get; set; }

        [MaxLength(20)]
        public string AssignCollectorEmpId { get; set; }

        [MaxLength(250)]
        public string AssignCollectorName { get; set; }

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

        public Guid? ReassignToId { get; set; }

        [MaxLength(20)]
        public string ReassignToEmpId { get; set; }

        [MaxLength(250)]
        public string ReassignToName { get; set; }

        public Guid? ReassignToTeamId { get; set; }

        [MaxLength(20)]
        public string ReassignToTeamCode { get; set; }

        [MaxLength(250)]
        public string ReassignToTeamName { get; set; }

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

        public bool? DoNotCallFlag { get; set; } = false;

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public DateTime? CreatedDate { get; set; } = DateTime.Now;

        [MaxLength(100)]
        public string UpdatedBy { get; set; }

        public DateTime? UpdatedDate { get; set; } = DateTime.Now;

        [Required]
        [MaxLength(10)]
        public string HistoryActionType { get; set; }

        [Required]
        public DateTime RecordedTimestamp { get; set; }

    }
}
