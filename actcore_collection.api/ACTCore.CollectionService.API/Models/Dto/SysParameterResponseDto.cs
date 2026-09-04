namespace ACTCore.CollectionService.API.Models.Dto
{
    public class SysParameterResponseDto
    {
        public int Id { get; set; }
        public string ParameterCategory { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
    }
}