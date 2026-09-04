namespace ACTCore.CollectionService.API.Models.Dto.LogDetailDto
{
    public class LogDetailUserBulkCreateDto
    {
        public Guid UserGroupId { get; set; }

        public DateTime? EffectiveDate { get; set; }

        public DateTime? ExpireDate { get; set; }

        public bool IsActive { get; set; }

        public bool ForceChangePassword { get; set; }

        public string EmployeeId { get; set; }

    }
}
