namespace ACTCore.CollectionService.API.Models.Dto
{
    public class PaymentDetailResponseDto
    {
        public DateTime PaymentDate { get; set; }
        public decimal PaymentAmount { get; set; }
        public string PaymentChannel { get; set; }
        public string ReferenceNo { get; set; }
        public decimal InstallAmount { get; set; }
        public decimal PenaltyAmount { get; set; }
        public decimal OtherFeesAmount { get; set; }
    }
}
