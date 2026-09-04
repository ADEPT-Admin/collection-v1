using ACTCore.CollectionService.API.Models;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using System.Net;
using System.Reflection;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ACTCore.CollectionService.API.Utilities
{
    public static class LoggerHelper
    {
        private static readonly JsonSerializerOptions ErrorMessageJsonOptions = new()
        {
            // Allow non-ASCII characters (e.g., Thai) without escaping to \uXXXX
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        };

        public static ActivityLog BuildActivityLog(string logAction, string entityId, ControllerContext controller, CurrentUserInfo userInfo, APIResponse response, string description = null)
        {
            var isSuccess = response.StatusCode == HttpStatusCode.OK;

            string? errorMessage = null;
            if (!isSuccess)
            {
                var payload = new
                {
                    Message = response.Message, // keep original object (En/Th)
                    ErrorMessages = response.ErrorMessages ?? new List<string>()
                };

                errorMessage = JsonSerializer.Serialize(payload, ErrorMessageJsonOptions);
            }

            var activityLog = new ActivityLog
            {
                UserId = userInfo.UserId,
                UserName = userInfo.UserName,
                UserGroup = userInfo.Roles,
                Action = logAction,
                EntityId = entityId,
                EntityName = $"{controller.ActionDescriptor.ControllerName}.{controller.ActionDescriptor.ActionName}",
                Description = description,
                Status = response.IsRequireConfirmation ? ActivityLogStatus.WARNING : (isSuccess ? ActivityLogStatus.SUCCESS : ActivityLogStatus.FAILED),
                ErrorMessage = errorMessage
            };
            return activityLog;
        }

        public static Dictionary<string, dynamic> BuildActivityCreateLog(dynamic obj)
        {
            if (obj == null)
                return new Dictionary<string, dynamic>();

            var dict = ((object)obj).GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(prop => prop.GetValue(obj) != null)
                .ToDictionary(
                    prop => prop.Name,
                    prop =>
                    {
                        var value = prop.GetValue(obj);
                        if (value is IEnumerable<Guid> guidList)
                            return string.Join(",", guidList);
                        if (value is System.Collections.IEnumerable && value is not string)
                            return JsonSerializer.Serialize(value, new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping });
                        return value.ToString();
                    }
                );
            return dict;
        }

        public static Dictionary<string, (string? OldValue, string? NewValue)> BuildActiviUpdateLog(dynamic beforeUpdateModel, dynamic afterUpdateModel)
        {
            if (beforeUpdateModel == null || afterUpdateModel == null)
                return new Dictionary<string, (string? OldValue, string? NewValue)>();

            var dict = new Dictionary<string, (string? OldValue, string? NewValue)>();

            var excludeFields = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "UpdatedBy",
                "UpdatedDate"
            };

            var dtoProps = ((object)beforeUpdateModel).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var modelProps = ((object)afterUpdateModel).GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (var prop in dtoProps)
            {
                if (excludeFields.Contains(prop.Name))
                    continue;

                var modelProp = modelProps.FirstOrDefault(p => p.Name == prop.Name);
                if (modelProp != null)
                {
                    var oldValue = modelProp.GetValue(beforeUpdateModel)?.ToString();
                    var newValue = prop.GetValue(afterUpdateModel)?.ToString();

                    if (oldValue != newValue)
                    {
                        dict[prop.Name] = (oldValue, newValue);
                    }
                }
            }

            return dict;
        }

        public static void SetActivityLogDetail(ActivityLog log, Dictionary<string, dynamic> changes)
        {
            foreach (var x in changes)
            {
                log.Details.Add(new ActivityLogDetail
                {
                    FieldName = x.Key,
                    NewValue = x.Value
                });
            }
        }
    }
}
