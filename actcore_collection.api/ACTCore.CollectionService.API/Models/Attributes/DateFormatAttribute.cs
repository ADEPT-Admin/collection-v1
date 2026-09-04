namespace ACTCore.CollectionService.API.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateFormatAttribute : Attribute
    {
        public string DateFormat { get; }

        public DateFormatAttribute(string format)
        {
            if (format == string.Empty)
                throw new ArgumentException("Date format cannot be null.");

            DateFormat = format;
        }
    }
}
