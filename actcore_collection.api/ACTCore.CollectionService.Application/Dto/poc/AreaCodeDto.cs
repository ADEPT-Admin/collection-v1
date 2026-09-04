namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class AreaCodeDto
    {
        public string AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int? AreaLevelId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubDistrictId { get; set; }
    }

    public class AreaCodeListingResponseDto
    {
        public string AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int? AreaLevelId { get; set; }
        public string AreaLevelName { get; set; }
        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string SubDistrictName { get; set; }
    }

    public class AreaCodeCreateDto
    {
        public string AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int? AreaLevelId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubDistrictId { get; set; }
    }

    public class AreaCodeUpdateDto
    {
        public string AreaId { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public int? AreaLevelId { get; set; }
        public int? ProvinceId { get; set; }
        public int? DistrictId { get; set; }
        public int? SubDistrictId { get; set; }
    }

    public class AreaCodeIdRequestDto
    {
        public string AreaId { get; set; }
    }
}
