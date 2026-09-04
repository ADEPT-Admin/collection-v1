namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ContractDetailResponseDto
    {
        public DateTime? ContractStartDate { get; set; }
        public DateTime? ContractEndDate { get; set; }
        public decimal? LoanAmount { get; set; }
        public decimal? InstallAmount { get; set; }
        public int? Term { get; set; }
        public decimal? InterestRate { get; set; }
        public string InterestRateType { get; set; }
        public DateTime? DueDate { get; set; }
        public string AssetGroup { get; set; }
        public string AssetType { get; set; }
        public string Description { get; set; }
        public decimal? AssetPrice { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string Series { get; set; }
        public int? Year { get; set; }
        public string EngineNo { get; set; }
        public string ChassisNo { get; set; }
        public string PlateNo { get; set; }
        public string RegisterProvince { get; set; }
    }
}
