namespace ACTCore.CollectionService.Application.Dto.poc
{
    public class AdjacentAreaDto
    {
        public int AdjacentAreaId { get; set; }
        public string AreaCode { get; set; }
        public string AdjacentAreaCode { get; set; }
        public AreaCodeDto Area { get; set; }
    }
    public class AdjacentAreaIdRequestDto
    {
        public int AdjacentAreaId { get; set; }
    }
    public class AdjacentAreaCreateDto
    {
        public string AreaCode { get; set; }
        public string AdjacentAreaCode { get; set; }
    }
    public class AdjacentAreaUpdateDto
    {
        public int AdjacentAreaId { get; set; }
        public string AreaCode { get; set; }
        public string AdjacentAreaCode { get; set; }
    }
}