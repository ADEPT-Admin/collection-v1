namespace ACTCore.CollectionService.Domain.ValueObjects
{
    public class MessageValue
    {
        public string Type { get; set; }

        public string KeyorText   { get; set; }

        public LanguageValue Value { get; set; }
    }
}
