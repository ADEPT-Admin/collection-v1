using CsvHelper.Configuration;

namespace ACTCore.CollectionService.Application.Helpers
{
    using CsvHelper;
    using CsvHelper.TypeConversion;
    using System.Globalization;

    public class ValidationConversionHelper : DefaultTypeConverter
    {
        private readonly string _dataType;
        private readonly bool _isRequired;
        private readonly string _fieldName;

        public ValidationConversionHelper(string dataType, bool isRequired, string fieldName)
        {
            _dataType = dataType.ToLowerInvariant();
            _isRequired = isRequired;
            _fieldName = fieldName;
        }

        public override object ConvertFromString(string text, IReaderRow row, MemberMapData memberMapData)
        {
            string raw = text?.Trim();

            // ---------- REQUIRED CHECK ----------
            if (_isRequired && string.IsNullOrWhiteSpace(raw))
            {
                throw new Exception($"Required field '{_fieldName}' is missing at row {row.Parser.Row}.");
            }

            // Treat NULL text as null
            if (string.IsNullOrWhiteSpace(raw) ||
                 raw.Equals("null", StringComparison.OrdinalIgnoreCase) ||
                 raw.Equals("nil", StringComparison.OrdinalIgnoreCase) ||
                 raw.Equals("none", StringComparison.OrdinalIgnoreCase)
               )
            {
                raw = null;
            }

            if (raw == null)
                return null;

            // ---------- DATATYPE VALIDATION ----------
            switch (_dataType)
            {
                case "string":
                    return raw;

                case "int":
                    if (int.TryParse(raw, out int i)) return i;
                    throw new Exception($"Invalid int value '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                case "long":
                    if (long.TryParse(raw, out long l)) return l;
                    throw new Exception($"Invalid long value '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                case "decimal":
                    if (decimal.TryParse(raw, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal d)) return d;
                    throw new Exception($"Invalid decimal value '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                case "bool":
                    if (raw == "1") return true;
                    if (raw == "0") return false;
                    if (bool.TryParse(raw, out bool b)) return b;
                    throw new Exception($"Invalid bool value '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                case "datetime":
                    if (DateTime.TryParse(raw, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt)) return dt;
                    throw new Exception($"Invalid datetime value '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                case "guid":
                    if (Guid.TryParse(raw, out Guid g)) return g;
                    throw new Exception($"Invalid GUID '{raw}' for field '{_fieldName}' at row {row.Parser.Row}");

                default:
                    throw new Exception($"Unknown DataType '{_dataType}' for field '{_fieldName}'");
            }
        }
    }

}
