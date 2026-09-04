namespace ACTCore.CollectionService.API.Models.Dto
{
    public class WorklistDetailResponseDto
    {
        public string ContractNo { get; set; }
        public string CustomerName { get; set; }
        public string NationalID { get; set; }
        public string PrimaryPhoneNo { get; set; }
        public string ContractStatus { get; set; }
        public int? DayPastDue { get; set; }
        public int? Bucket { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? LastPaymentDate  { get; set; }
        public decimal? OverdueAmount { get; set; }
        public decimal? OutstandingBalance  { get; set; }
    }
}
