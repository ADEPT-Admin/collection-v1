namespace ACTCore.CollectionService.API.Models.Dto.ListPagedDto
{
    public class CollectorListPagedResponseDto
    {
        public Guid CollectorGuid { get; set; }

        public string CollectorId { get; set; }

        public string CollectorName { get; set; }

        public string ColRoleName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public string IsActive { get; set; }
    }
}
