namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class CollectionNoteDto
    {
        public int Id { get; set; }
        public string ContractNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ProcessType { get; set; }
        public string FollowupResult { get; set; }
        public string ContractPerson { get; set; }
        public string Remark { get; set; }
        public string HandleBy { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public decimal? TotalPaymentAmountInVat { get; set; }
    }

    public class CollectionNoteCreateDto
    {
        public string ContractNo { get; set; }
        public string ProcessType { get; set; }
        public string ContractPerson { get; set; }
        public string PhoneNo { get; set; }
        public string FollowupResult { get; set; }
        public string Remark { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public decimal? TotalPaymentAmountInVat { get; set; }
    }

    public class CollectionNoteUpdateDto
    {
        public int Id { get; set; }
        public string ContractNo { get; set; }
        public DateTime TransactionDate { get; set; }
        public string ProcessType { get; set; }
        public string FollowupResult { get; set; }
        public string ContractPerson { get; set; }
        public string Remark { get; set; }
        public string HandleBy { get; set; }
        public DateTime? AppointmentDate { get; set; }
        public decimal? TotalPaymentAmountInVat { get; set; }
    }

    public class CollectionNoteIdRequestDto
    {
        public int Id { get; set; }
    }
}