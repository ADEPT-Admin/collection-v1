using SharedKernel.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[Table("WorkFlowStep")]
[Index(nameof(WorkFlowStepCode), IsUnique = true)]
public partial class WorkFlowStep : VersionBaseModel
{
    [Key]
    [Column("ID")]

    public Guid Id { get; set; }

    [Required]
    [Column("WorkFlowID")]
    public Guid WorkFlowId { get; set; }

    [Required]
    [MaxLength(10)]
    public string WorkFlowCode { get; set; }

    [Required]
    [MaxLength(10)]
    public string WorkFlowStepCode { get; set; }

    public int StepNumber { get; set; }

    [MaxLength(100)]
    public string StepName { get; set; }

    public string ActionList { get; set; }

    [ForeignKey(nameof(WorkFlowId))]
    public virtual WorkFlow WorkFlow { get; set; }

    public virtual ICollection<WorkFlowApplication> WorkFlowApplications { get; set; }
}
