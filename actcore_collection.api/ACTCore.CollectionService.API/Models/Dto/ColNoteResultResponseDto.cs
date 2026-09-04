namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColNoteResultResponseDto
    {
        public int ResultId { get; set; }
        public int ActionId { get; set; }
        public string ResultCode { get; set; }
        public string ResultDescription { get; set; }
        public bool IsActive { get; set; }
    }
}
