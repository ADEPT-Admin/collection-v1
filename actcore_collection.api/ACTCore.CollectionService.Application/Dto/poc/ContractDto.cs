using ACTCore.CollectionService.Domain.Master;

namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class ContractDto
    {
        public string ContractNo { get; set; }
        public string CustomerNo { get; set; }
        public int PrefixId { get; set; }
        public Prefix Prefix { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public int? Age { get; set; }
        public string LoanType { get; set; }
        public DateTime? ContractDate { get; set; }
        public int ContractStatus { get; set; }
        public string ContractStatusDesc { get; set; } // Active/Inactive
        public string RiskLevel { get; set; }
        //public List<ContractAddressDto> ContractAddresses { get; set; }
        //public List<ContractAssetInfoDto> ContractAssetInfo { get; set; }
        //public List<ContractDetailDto> ContractDetails { get; set; }
        //public List<ContractPaymentHistoryDto> ContractPaymentHistories { get; set; }
        //public List<ContractPersonInfoDto> ContractPersons { get; set; }
        //public List<ContractPhoneDto> ContractPhones { get; set; }
        //public List<CollectionNoteDto> CollectionNotes { get; set; }
    }

    public class ContractIdRequestDto
    {
        public string ContractNo { get; set; }
    }

    public class ContractCreateDto
    {
        public string ContractNo { get; set; }
        public string CustomerNo { get; set; }
        public int PrefixId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public int? Age { get; set; }
        public string LoanType { get; set; }
        public DateTime? ContractDate { get; set; }
        public int ContractStatus { get; set; }
        public string ContractStatusDesc { get; set; }
        public string RiskLevel { get; set; }
    }

    public class ContractUpdateDto
    {
        public string ContractNo { get; set; }
        public string CustomerNo { get; set; }
        public int PrefixId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string IDCard { get; set; }
        public int? Age { get; set; }
        public string LoanType { get; set; }
        public DateTime? ContractDate { get; set; }
        public int ContractStatus { get; set; }
        public string ContractStatusDesc { get; set; }
        public string RiskLevel { get; set; }
    }


}