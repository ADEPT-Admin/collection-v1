namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class EnumDto
    {
        public int EnumId { get; set; }
        public string EnumName { get; set; }
        public string EnumCode { get; set; }
        public string EnumDescription { get; set; }
        public int? EnumOrder { get; set; }
        public string Note { get; set; }
    }

    public class EnumListingResponseDto
    {
        public int EnumId { get; set; }
        public string EnumName { get; set; }
        public string EnumCode { get; set; }
        public string EnumDescription { get; set; }
        public int? EnumOrder { get; set; }
        public string Note { get; set; }
    }

    public class EnumCreateDto
    {
        public string EnumName { get; set; }
        public string EnumCode { get; set; }
        public string EnumDescription { get; set; }
        public int? EnumOrder { get; set; }
        public string Note { get; set; }
    }

    public class EunUpdateDto
    {
        public int EnumId { get; set; }
        public string EnumName { get; set; }
        public string EnumCode { get; set; }
        public string EnumDescription { get; set; }
        public int? EnumOrder { get; set; }
        public string Note { get; set; }
    }

    public class EnumIdRequestDto
    {
        public string EnumId { get; set; }
    }
}
