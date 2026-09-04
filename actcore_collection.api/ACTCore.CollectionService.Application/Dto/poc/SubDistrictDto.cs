namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class SubDistrictDto
    {
        public int SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictNameEn { get; set; }
        public float? Latitude { get; set; }
        public float? Longtitude { get; set; }
        public string ZipCode { get; set; }
        public DistrictDto District { get; set; }
        public List<AreaCodeDto> Areas { get; set; }
    }

    public class SubDistrictIdRequestDto
    {
        public int SubDistrictId { get; set; }
    }

    public class SubDistrictCreateDto
    {
        public int? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictNameEn { get; set; }
        public float? Latitude { get; set; }
        public float? Longtitude { get; set; }
        public string ZipCode { get; set; }
    }

    public class SubDistrictUpdateDto
    {
        public int SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictNameEn { get; set; }
        public float? Latitude { get; set; }
        public float? Longtitude { get; set; }
        public string ZipCode { get; set; }
    }

    public class SubDistrictListingResponseDto
    {
        public int SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictNameEn { get; set; }
        public float? Latitude { get; set; }
        public float? Longtitude { get; set; }
        public string ZipCode { get; set; }
    }

    public class SubDistrictDetailResponseDto
    {
        public int SubDistrictId { get; set; }
        public int? DistrictId { get; set; }
        public string SubDistrictCode { get; set; }
        public string SubDistrictName { get; set; }
        public string SubDistrictNameEn { get; set; }
        public float? Latitude { get; set; }
        public float? Longtitude { get; set; }
        public string ZipCode { get; set; }
        public string DistrictName { get; set; }
    }
}