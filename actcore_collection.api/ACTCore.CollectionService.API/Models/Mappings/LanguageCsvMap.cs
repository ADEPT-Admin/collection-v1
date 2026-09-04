using ACTCore.CollectionService.API.Models.Dto;
using CsvHelper.Configuration;

namespace ACTCore.CollectionService.API.Models.Mappings
{
    public class LanguageCsvMap: ClassMap<LanguageCreateDto>
    {
        public LanguageCsvMap() 
        { 
            Map(m => m.Key).Name("Key");
            Map(m => m.Value.En).Name("EN");
            Map(m => m.Value.Th).Name("TH");
        }
    }
}
