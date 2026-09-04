namespace ACTCore.CollectionService.API.Models.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ColumnWidthAttribute : Attribute
    {
        public int Width { get; }

        public ColumnWidthAttribute(int width)
        {
            if (width <= 0)
                throw new ArgumentException("Column width must be greater than zero.", nameof(width));

            Width = width;
        }
    }
}
