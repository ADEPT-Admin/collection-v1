using ACTCore.CollectionService.Domain.ValueObjects;

namespace ACTCore.CollectionService.Application.Dto
{
    public class DepartmentDto
    {
        public int DepartmentId { get; set; }
        public string DepartmentCode { get; set; }
        public LanguageValue NameLanguage { get; set; }
    }
}

