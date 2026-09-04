using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class CollectorDisplayFilterActiveDto
    {
        [ColumnWidth(120)]
        public string CollectorId { get; set; }

        public string CollectorName { get; set; }

        [ColumnWidth(200)]
        public string ColRoleName { get; set; }

        public string Email { get; set; }

        public string PhoneNo { get; set; }

        public bool IsActive { get; set; }
    }
}
