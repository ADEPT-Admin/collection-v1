using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class SysParameterDisplayFilterDto
    {
        [ColumnWidth(200)]
        public string ParameterCategory { get; set; }
        [ColumnWidth(250)]
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public string Description { get; set; }
        public bool IsSystem { get; set; }
    }
}
