using ACTCore.CollectionService.Domain.Entities.Imports;
using CsvHelper.Configuration;
using System.Reflection;


namespace ACTCore.CollectionService.Application.Helpers
{
    public class DynamicMappingHelper<T> : ClassMap<T> where T : class, new()
    {
        public DynamicMappingHelper(IEnumerable<MappingImportDetail> mappings)
        {
            // หา method แบบ Map(Type, MemberInfo, bool)
            var mapMethod = typeof(ClassMap<T>)
                .GetMethod("Map", new[] { typeof(Type), typeof(MemberInfo), typeof(bool) });

            if (mapMethod == null)
                throw new Exception("Cannot find Map(Type, MemberInfo, bool) method in CsvHelper.");

            foreach (var map in mappings)
            {
                var prop = typeof(T).GetProperty(map.FieldName);
                if (prop == null)
                    throw new Exception($"Property {map.FieldName} not found in {typeof(T).Name}");

                // call: Map(typeof(T), prop, false)
                var memberMap = (MemberMap)mapMethod.Invoke(this, new object[] { typeof(T), prop, false });

                // apply Name("column")
                memberMap.Name(map.MappingField);

                memberMap.TypeConverter(
                    new ValidationConversionHelper(
                        map.DataType,
                        map.IsRequired,
                        map.FieldName
                    )
                );
            }
        }
    }
}
