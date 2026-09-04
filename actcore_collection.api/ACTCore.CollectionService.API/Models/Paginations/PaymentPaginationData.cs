namespace ACTCore.CollectionService.API.Models.Paginations
{
    public class PaymentPaginationData<T>: PaginationData<T>
    {
        public int? ContractTerm { get; set; }
        public int? PaidTerm { get; set; }
        public decimal? PaidAmount { get; set; }
    }
}
