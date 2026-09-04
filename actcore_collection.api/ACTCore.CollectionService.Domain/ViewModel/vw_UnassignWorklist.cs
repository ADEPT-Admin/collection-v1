using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ACTCore.CollectionService.Domain.ViewModel
{
    [Keyless]
    public class vw_UnassignWorklist
    {
        public Guid WorklistId { get; set; }

        [MaxLength(20)]
        public string ContractNo { get; set; }

        [MaxLength(250)]
        public string CustomerName { get; set; }

        [MaxLength(250)]
        public string AssetType { get; set; }

        [MaxLength(250)]
        public string ContractStatus { get; set; }

        public int? Bucket { get; set; }

        public int? PaymentDueDate { get; set; }

        public int? DayPastDue { get; set; }

        public decimal? OverdueAmount { get; set; }

        public decimal? OutstandingBalance { get; set; }

        [MaxLength(250)]
        public string CollectorName { get; set; }

        [MaxLength(250)]
        public string TeamName { get; set; }

        [MaxLength(250)]
        public string ReassignFrom { get; set; }

        public DateTime? AssignDate { get; set; }

        [MaxLength(250)]
        public string FollowupStatus { get; set; }

        public DateTime? LastFollowupDate { get; set; }

    }
}
