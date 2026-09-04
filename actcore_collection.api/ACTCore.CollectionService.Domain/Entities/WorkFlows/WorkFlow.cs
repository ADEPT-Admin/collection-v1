using Microsoft.EntityFrameworkCore;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[Table("WorkFlow")]
[Index(nameof(WorkFlowCode), IsUnique = true)]
public partial class WorkFlow : VersionBaseModel
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [Required]
    [Column("WorkFlowStepCode")]
    [MaxLength(10)]
    public string WorkFlowCode { get; set; }

    [Required]
    [MaxLength(100)]
    public string WorkFlowName { get; set; }

    public virtual ICollection<WorkFlowStep> WorkFlowSteps { get; set; } = new List<WorkFlowStep>();
}
