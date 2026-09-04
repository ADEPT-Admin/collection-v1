using System.Reflection;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class ModelHelper
    {
        public static T CloneModel<T>(T source) where T : class, new()
        {
            if (source == null) return null;

            var clone = new T();
            var type = typeof(T);

            foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (!prop.CanWrite) continue;
                var value = prop.GetValue(source);

                // Handle collections of objects
                if (value is System.Collections.IEnumerable enumerable && !(value is string))
                {
                    var listType = prop.PropertyType;
                    var elementType = listType.IsGenericType ? listType.GetGenericArguments()[0] : typeof(object);
                    var clonedList = (System.Collections.IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));
                    foreach (var item in enumerable)
                    {
                        // Recursively clone if possible
                        var cloneMethod = typeof(ModelHelper).GetMethod(nameof(CloneModel)).MakeGenericMethod(elementType);
                        var clonedItem = cloneMethod.Invoke(null, new[] { item });
                        clonedList.Add(clonedItem);
                    }
                    prop.SetValue(clone, clonedList);
                }
                else
                {
                    prop.SetValue(clone, value);
                }
            }
            return clone;
        }
    }
}
