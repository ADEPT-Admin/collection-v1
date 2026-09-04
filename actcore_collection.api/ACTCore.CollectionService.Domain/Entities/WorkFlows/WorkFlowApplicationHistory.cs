using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[PrimaryKey("ApplicationId", "ApplicationSubmitDate")]
[Table("WorkFlowApplicationHistory")]
public partial class WorkFlowApplicationHistory
{
    [Key]
    [Column("ApplicationID")]
    public Guid ApplicationId { get; set; }

    [Key]
    public DateTime ApplicationSubmitDate { get; set; }

    [Column("WorkFlowStepID")]
    public Guid WorkFlowStepId { get; set; }

    public Guid WorkFlowStepCode { get; set; }

    [MaxLength(1)]
    public string ApplicationStatus { get; set; }

    [Column("PreviousWorkFlowStepID")]
    public Guid PreviousWorkFlowStepId { get; set; }

    [Column("PreviousWorkFlowStepCode")]
    public Guid PreviousWorkFlowStepCode { get; set; }

    [MaxLength(2)]
    public string PreviousAction { get; set; }

    [ForeignKey(nameof(ApplicationId))]
    public virtual WorkFlowApplication WorkFlowApplications { get; set; }
}
