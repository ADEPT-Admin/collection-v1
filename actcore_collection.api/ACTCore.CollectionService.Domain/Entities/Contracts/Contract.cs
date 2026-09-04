using ACTCore.CollectionService.Domain.Entities.Collections;
using SharedKernel.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ACTCore.CollectionService.Domain.Entities.Contracts
{
    [Table("Contract")]
    public class Contract : VersionBaseModel
    {
        [Key]
        [MaxLength(20)]
        public string ContractNo { get; set; }

        [MaxLength(50)]
        public string LoanType { get; set; }

        public DateTime? ContractStartDate { get; set; }

        public DateTime? ContractEndDate { get; set; }

        public decimal? InterestRate { get; set; }

        [MaxLength(50)]
        public string InterestRateType { get; set; }

        public int? PaymentDueDate { get; set; }

        public DateTime? DueDate { get; set; }

        public decimal? LoanAmount { get; set; }

        public int? Term { get; set; }

        public decimal? InstallAmount { get; set; }

        public int? ContractMOB { get; set; }

        public int? Bucket { get; set; }

        public int? DayPastDue { get; set; }

        public decimal? OverdueAmount { get; set; }

        public decimal? OutstandingBalance { get; set; }

        public int? PaymentReceivedTerm { get; set; }

        public decimal? PaymentReceivedAmount { get; set; }

        public DateTime? LastPaymentDate { get; set; }

        public string ContractStatus { get; set; }

        public string RiskLevel { get; set; }

        // Navigation property (collection)
        public ICollection<Worklist> Worklists { get; set; } = new List<Worklist>();
        public ICollection<ContractPerson> ContractPersons { get; set; } = new List<ContractPerson>();
        public ICollection<ContractAsset> ContractAssets { get; set; } = new List<ContractAsset>();
        public ICollection<ContractAssetVehicle> ContractAssetVehicles { get; set; } = new List<ContractAssetVehicle>();
        public ICollection<ContractOverdue> ContractOverdues { get; set; } = new List<ContractOverdue>();
        public ICollection<ContractPayment> ContractPayments { get; set; } = new List<ContractPayment>();
    }
}
