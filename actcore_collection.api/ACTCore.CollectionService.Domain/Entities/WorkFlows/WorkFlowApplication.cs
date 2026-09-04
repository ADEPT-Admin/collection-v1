using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[Table("WorkFlowApplication")]
public partial class WorkFlowApplication : VersionBaseModel
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Required]
    [Column("ApplicationID")]
    public Guid ApplicationId { get; set; }

    [Required]
    [Column("WorkFlowID")]
    public Guid WorkFlowId { get; set; }

    [Required]
    [MaxLength(10)]
    public string WorkFlowCode { get; set; }

    [Required]
    [Column("WorkFlowStepID")]
    public Guid WorkFlowStepId { get; set; }

    [Required]
    [MaxLength(10)]
    public string WorkFlowStepCode { get; set; }

    public DateTime? EntryStepDate { get; set; }

    [MaxLength(1)]
    public string ApplicationStatus { get; set; }

    [ForeignKey(nameof(WorkFlowId))]
    public virtual WorkFlow WorkFlow { get; set; }

    [ForeignKey(nameof(WorkFlowStepId))]
    public virtual WorkFlowStep WorkFlowStep { get; set; }

    public virtual ICollection<WorkFlowApplicationHistory> WorkFlowApplicationHistories { get; set; }
}
