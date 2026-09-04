namespace ACTCore.CollectionService.API.Models.Dto
{
    public class ManualReassignRequestDto
    {
        public Guid WorklistId { get; set; }
        public Guid CollectorId { get; set; }
        public string ReassignReason { get; set; }
    }
}
