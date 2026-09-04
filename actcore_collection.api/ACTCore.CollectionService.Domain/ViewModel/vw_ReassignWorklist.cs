using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Domain.ViewModel
{
    [Keyless]
    public class vw_ReassignWorklist
    {
        public Guid WorklistId { get; set; }

        public Guid? AssignCollectorId { get; set; }

        [MaxLength(250)]
        public string AssignCollectorName { get; set; }

        [MaxLength(20)]
        public string ContractNo { get; set; }

        [MaxLength(250)]
        public string CustomerName { get; set; }

        [MaxLength(250)]
        public string AssetType { get; set; }

        [MaxLength(250)]
        public string ContractStatus { get; set; }

        [MaxLength(250)]
        public string FollowupStatus { get; set; }

        public Guid? ReassignById { get; set; }

        [MaxLength(250)]
        public string ReassignBy { get; set; }

        public Guid? ReassignFromId { get; set; }

        [MaxLength(250)]
        public string ReassignFrom { get; set; }

        public Guid? AssignTeamId { get; set; }

        [MaxLength(250)]
        public string TeamFrom { get; set; }

        public Guid? ReassignToId { get; set; }

        [MaxLength(250)]
        public string ReassignTo { get; set; }

        public Guid? ReassignToTeamId { get; set; }

        [MaxLength(250)]
        public string TeamTo { get; set; }

        [MaxLength(250)]
        public DateTime RequestedDate { get; set; }

        [MaxLength(250)]
        public string Reason { get; set; }

        [MaxLength(250)]
        public string ReassignStatus { get; set; }

        public bool IsMyTeam { get; set; }
    }
}
