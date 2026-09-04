namespace ACTCore.CollectionService.API.Models.Dto.LogDetailDto
{
    public class LogDetailCollectorBulkCreateDto
    {
        public Guid UserId { get; set; }
        public Guid ColRoleId { get; set; }
        public bool IsActive { get; set; }

    }
}