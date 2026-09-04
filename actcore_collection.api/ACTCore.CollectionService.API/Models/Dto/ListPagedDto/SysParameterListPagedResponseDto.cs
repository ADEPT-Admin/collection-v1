namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class SysParameterListPagedResponseDto
    {
        public int Id { get; set; }
        public string ParameterCategory { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Description { get; set; }
        public string IsSystem { get; set; }
    }
}