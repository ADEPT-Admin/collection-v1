namespace ACTCore.CollectionService.Domain.ValueObjects
{
    public class DisplayColumnPropertyOverdueValue
    {
        public string Key { get; set; }
        public LanguageValue DisplayColumnName { get; set; }
        public string Type { get; set; }
        public int? ColumnWidth { get; set; }
        public string DateFormat { get; set; }
    }
}