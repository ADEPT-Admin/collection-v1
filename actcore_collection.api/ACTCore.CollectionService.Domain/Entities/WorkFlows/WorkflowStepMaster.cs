using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[Table("WorkflowStepMaster")]
public partial class WorkflowStepMaster
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [MaxLength(10)]
    public string StepNumber { get; set; }

    [MaxLength(80)]
    public string StepName { get; set; }
}
