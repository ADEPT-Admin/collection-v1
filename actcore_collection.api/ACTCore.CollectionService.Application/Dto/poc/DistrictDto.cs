namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class DistrictDto
    {
        public int DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEn { get; set; }
        public List<SubDistrictDto> SubDistricts { get; set; }
        public List<AreaCodeDto> Areas { get; set; }
    }

    public class DistrictIdRequestDto
    {
        public int DistrictId { get; set; }
    }

    public class DistrictCreateDto
    {
        public int? ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEn { get; set; }
    }

    public class DistrictUpdateDto
    {
        public int DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEn { get; set; }
    }

    public class DistrictListingResponseDto
    {
        public int DistrictId { get; set; }
        public int? ProvinceId { get; set; }
        public string DistrictCode { get; set; }
        public string DistrictName { get; set; }
        public string DistrictNameEn { get; set; }
    }
}