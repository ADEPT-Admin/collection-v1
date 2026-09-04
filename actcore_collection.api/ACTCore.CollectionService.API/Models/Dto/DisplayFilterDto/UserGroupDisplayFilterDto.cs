using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class UserGroupDisplayFilterDto
    {
        [ColumnWidth(150)]
        public string UserGroupCode { get; set; }

        [ColumnWidth(300)]
        public string UserGroupName { get; set; }

        public string Description { get; set; }

        public bool IsActive { get; set; }
    }
}
