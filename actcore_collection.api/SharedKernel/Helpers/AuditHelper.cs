using System.Reflection;

namespace SharedKernel.Helpers
{
    public static class AuditHelper
    {
        public static Dictionary<string, (string? OldValue, string? NewValue)> GetFieldChanges<TModel, TDto>(TModel model, TDto dto)
        {
            var changes = new Dictionary<string, (string?, string?)>();

            if (model == null || dto == null)
                return changes;

            var modelProps = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var dtoProps = typeof(TDto).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            var dtoPropDict = dtoProps.ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            foreach (var modelProp in modelProps)
            {
                if (!modelProp.CanRead || !dtoPropDict.TryGetValue(modelProp.Name, out var dtoProp))
                    continue;

                if (!dtoProp.CanRead) continue;

                var oldVal = modelProp.GetValue(model)?.ToString();
                var newVal = dtoProp.GetValue(dto)?.ToString();

                if (oldVal != newVal)
                {
                    changes[modelProp.Name] = (oldVal, newVal);
                }
            }

            return changes;
        }
    }

}
