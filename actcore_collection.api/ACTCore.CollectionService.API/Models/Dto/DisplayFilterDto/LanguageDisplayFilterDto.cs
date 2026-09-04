using ACTCore.CollectionService.API.Models.Attributes;

namespace ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto
{
    public class LanguageDisplayFilterDto
    {
        [ColumnWidth(150)]
        public string Key { get; set; }

        public string ValueEn { get; set; }

        public string ValueTh { get; set; }
    }
}
