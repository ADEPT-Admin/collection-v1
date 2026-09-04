namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ApproveReassignRequestDto
    {
        public Guid WorklistId { get; set; }
        public string ApproveReason { get; set; }
        public string Type { get; set; } = "A";
    }
}
