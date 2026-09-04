using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.WorkFlows;

[Table("WorkflowActionListMaster")]
public partial class WorkflowActionListMaster : VersionBaseModel
{
    [Key]
    [Column("ID")]
    public Guid Id { get; set; }

    [MaxLength(10)]
    public string ActionCode { get; set; }

    [MaxLength(250)]
    public string ActionName { get; set; }
}
