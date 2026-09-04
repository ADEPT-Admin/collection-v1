using ACTCore.CollectionService.API.Models.Attributes;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Domain.ValueObjects;
using SharedKernel.CommonConstants;
using SharedKernel.Templates;
using System.Reflection;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class DataTableHelper
    {
        /// <summary>
        /// Gets filterable properties for a given DTO type.
        /// </summary>
        /// <typeparam name="T">DTO type</typeparam>
        /// <param name="propertyNames">Optional: restrict to these property names</param>
        /// <returns>List of FilterValue</returns>
        public static IEnumerable<DisplayColumnPropertyValue> GetdisplayProperties<T>(IEnumerable<string> propertyNames = null)
        {
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            if (propertyNames != null)
            {
                props = props.Where(p => propertyNames.Contains(p.Name)).ToArray();
            }

            return props.Select(p => new DisplayColumnPropertyValue
            {
                Key = p.Name,
                Type = Nullable.GetUnderlyingType(p.PropertyType)?.Name ?? p.PropertyType.Name
            }).ToList();
        }


        public static IEnumerable<DisplayColumnPropertyValue> GetDisplayColumnAndDisplayProperties<T>(EnumValue[] enumValues = null)
        {
            // Do not return properties whose type is a custom DTO (i.e., user-defined class in the same assembly/namespace)
            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p =>
                {
                    var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                    // Exclude custom DTOs (user-defined classes, not system types, not enums, not primitives)
                    if (type.IsClass && type != typeof(string) && !type.Namespace.StartsWith("System"))
                        return false;
                    return true;
                })
                .ToList(); // Ensure order is preserved

            return props.Select(p =>
            {
                var type = Nullable.GetUnderlyingType(p.PropertyType) ?? p.PropertyType;
                var typeCode = Type.GetTypeCode(type);
                string typeName;

                // Read ColumnWidthAttribute if present
                var columnWidthAttr = p.GetCustomAttribute<ColumnWidthAttribute>(inherit: false);
                var columnWidth = columnWidthAttr?.Width;

                var dateFormatAttr = p.GetCustomAttribute<DateFormatAttribute>(inherit: false);
                var dateFormat = dateFormatAttr?.DateFormat;

                if (type == typeof(bool))
                {
                    typeName = "Enum";
                    columnWidth = 80;
                }
                else if (type == typeof(DateOnly))
                {
                    typeName = "Date";
                    columnWidth = 120;
                }
                else if (type == typeof(DateTime))
                {
                    typeName = type.Name;
                    columnWidth = 150;
                }
                else if (type == typeof(int))
                {
                    typeName = "Integer";
                }
                else if (type == typeof(decimal))
                {
                    typeName = "Decimal";
                }
                else
                {
                    typeName = type.Name;
                }

                return new DisplayColumnPropertyValue
                {
                    Key = p.Name,
                    Type = typeName,
                    Values = type == typeof(bool) ? enumValues : null,
                    ColumnWidth = columnWidth,
                    DateFormat = dateFormat != null ? dateFormat : null
                };
            }).ToList();
        }

        public async static Task<IEnumerable<DisplayColumnPropertyOverdueValue>> GetDisplayColumnByContractOverdueSummary(
            Dictionary<string, object> data, IEnumerable<SysEnum> overdueTypeEnums, LanguageService languageService)
        {
            if (data == null || data.Count == 0)
                return Enumerable.Empty<DisplayColumnPropertyOverdueValue>();

            var keys = data.Keys.ToList();
            var languageResults = await languageService.GetListAsync(x => keys.Contains(x.Key)); // List of Language entities
            var languageDict = languageResults?.ToDictionary(l => l.Key) ?? new Dictionary<string, Language>();

            var enumDict = overdueTypeEnums.ToDictionary(e => e.EnumCode, e => e);

            return data.Select(kvp =>
            {
                var value = kvp.Value;
                var type = value?.GetType() ?? typeof(string);

                type = Nullable.GetUnderlyingType(type) ?? type;

                string typeName;
                int? columnWidth = null;
                string dateFormat = null;

                if (type == typeof(bool))
                {
                    typeName = "Enum";
                    columnWidth = 80;
                }
                else if (type == typeof(DateTime) || type == typeof(DateOnly))
                {
                    typeName = "Date";
                    columnWidth = 120;
                    dateFormat = CommonConstants.DateFormat;
                }
                else if (type == typeof(int))
                {
                    typeName = "Integer";
                }
                else if (type == typeof(decimal))
                {
                    typeName = "Decimal";
                }
                else
                {
                    typeName = type.Name;
                }

                var dynamicOverdueLanguage = enumDict.TryGetValue(kvp.Key, out var enumValue) && enumValue?.EnumDescription != null
                    ? new LanguageValue
                    {
                        Th = enumValue.EnumDescription,
                        En = enumValue.EnumDescriptionEn ?? enumValue.EnumDescription
                    }
                    : languageDict.TryGetValue(kvp.Key, out var langValue) && langValue != null
                    ? new LanguageValue
                    {
                        Th = langValue.ValueTh,
                        En = langValue.ValueEn
                    }
                    : new LanguageValue
                    {
                        Th = kvp.Key,
                        En = kvp.Key
                    };

                return new DisplayColumnPropertyOverdueValue
                {
                    Key = kvp.Key,
                    DisplayColumnName = dynamicOverdueLanguage,
                    Type = typeName,
                    ColumnWidth = columnWidth,
                    DateFormat = dateFormat
                };
            }).ToList();
        }
    }
}
