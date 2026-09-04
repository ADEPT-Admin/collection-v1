namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class ProvinceDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceAbbr { get; set; }
        public string ProvinceNameEn { get; set; }
        public string ProvinceAbbrEn { get; set; }
        public string RegionCode { get; set; }
        public List<AreaCodeDto> Areas { get; set; }
        public List<DistrictDto> Districts { get; set; }
    }

    public class ProvinceIdRequestDto
    {
        public int ProvinceId { get; set; }
    }

    public class ProvinceCreateDto
    {
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceAbbr { get; set; }
        public string ProvinceNameEn { get; set; }
        public string ProvinceAbbrEn { get; set; }
        public string RegionCode { get; set; }
    }

    public class ProvinceUpdateDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceAbbr { get; set; }
        public string ProvinceNameEn { get; set; }
        public string ProvinceAbbrEn { get; set; }
        public string RegionCode { get; set; }
    }
    public class ProvinceListingResponseDto
    {
        public int ProvinceId { get; set; }
        public string ProvinceCode { get; set; }
        public string ProvinceName { get; set; }
        public string ProvinceAbbr { get; set; }
        public string ProvinceNameEn { get; set; }
        public string ProvinceAbbrEn { get; set; }
        public string RegionCode { get; set; }
    }

}