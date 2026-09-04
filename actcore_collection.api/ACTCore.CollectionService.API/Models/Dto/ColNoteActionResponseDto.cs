namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ColNoteActionResponseDto
    {
        public int ActionId { get; set; }
        public string ActionCode { get; set; }
        public string ActionDescription { get; set; }

        public bool IsActive { get; set; }
    }
}
