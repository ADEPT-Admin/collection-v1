namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class WorklistDto
    {
        public int Id { get; set; }
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public DateTime? DueDate { get; set; }
        public decimal OverdueAmount { get; set; }
        public string Bucket { get; set; }
        public string JobType { get; set; }
        public string ProductGroup { get; set; }
        public string SubProduct { get; set; }
        public string RiskLevel { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public DateTime? LastContractDate { get; set; }
        public string ResultCode { get; set; }
        public string CollectorId { get; set; }
        public string CollectorName { get; set; }
        public string SupervisorId { get; set; }
        public string SupervisorName { get; set; }
        public int FollowupStatus { get; set; }
        public string FollowupStatusDesc { get; set; }
        public DateTime? AllocateDate { get; set; }
        public string ReassignFrom { get; set; }
        public string ReassignTo { get; set; }
        public string ReassignReason { get; set; }
        public int FollowupStatusOrder { get; set; }
    }

    public class ContractRequestDto
    {
        public string ContractNo { get; set; }
    }

    public class ContractRequestActionDto
    {
        public string ContractNo { get; set; }
        public string ReassignReason { get; set; }
    }

    public class WorkListRequestDto
    {
        public int Id { get; set; }
    }
}