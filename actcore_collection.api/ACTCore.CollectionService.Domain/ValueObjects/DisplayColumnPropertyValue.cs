
using SharedKernel.Templates;

namespace ACTCore.CollectionService.Domain.ValueObjects
{
    public class DisplayColumnPropertyValue
    {
        public string Key { get; set; }
        public string Type { get; set; }
        public EnumValue[] Values { get; set; }
        public int? ColumnWidth { get; set; }
        public string DateFormat { get; set; }
    }
}
