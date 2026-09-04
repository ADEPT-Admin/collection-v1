namespace ACTCore.CollectionService.API.Models.Dto
{
    #nullable enable
    public class SysPolicyResponseDto
    {
        public int Id { get; set; }
        public string? PolicyCategory { get; set; }
        public string? PolicyCode { get; set; }
        public string? PolicyName { get; set; }
        public string? PolicyValue { get; set; }
        public bool IsActive { get; set; }
    }
}
