using Microsoft.Data.SqlClient;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using SharedKernel.Templates;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace SharedKernel.Helpers
{
    public static class FilterHelper
    {
        public static Expression<Func<T, bool>> BuildFilterExpression<T>(FilterContainer filterContainer)
        {
            if (filterContainer?.DynamicFilters is not { Count: > 0 })
                return null;

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression combinedExpression = Expression.Constant(true);

            foreach (var kvp in filterContainer.DynamicFilters)
            {
                var propertyName = kvp.Key;
                var item = kvp.Value;
                if (item == null) continue;

                var property = Expression.Property(parameter, propertyName);
                Expression condition = null;

                switch (item?.Type?.ToUpper())
                {
                    case FilterType.INTEGER:
                    case FilterType.DECIMAL:
                        condition = BuildNumberExpression(property, item);
                        break;

                    case FilterType.DATE:
                    case FilterType.DATETIME:
                        condition = BuildDateExpression(property, item);
                        break;
                    case FilterType.ENUM:
                        condition = BuildBoolExpression(property, item);
                        break;
                    case FilterType.GUID:
                        condition = BuildGuidExpression(property, item);
                        break;
                case FilterType.STRING:
                default:
                        condition = BuildStringExpression(property, item);
                        break;
                }

                if (condition != null)
                    combinedExpression = Expression.AndAlso(combinedExpression, condition);
            }

            return Expression.Lambda<Func<T, bool>>(combinedExpression, parameter);
        }

        private static Expression BuildStringExpression(MemberExpression property, FilterItem item)
        {
            var value = item.Value?.ToString() ?? "";
            var constant = Expression.Constant(value);

            return Expression.Call(property, nameof(string.Contains), null, constant);
        }

        private static Expression BuildGuidExpression(MemberExpression property, FilterItem item)
        {
            if (item.Value == null || !Guid.TryParse(item.Value.ToString(), out var guidValue))
                return null;
            var constant = Expression.Constant(guidValue, property.Type);
            return item.Operator switch
            {
                "=" => Expression.Equal(property, constant),
                _ => null
            };
        }

        private static Expression BuildNumberExpression(MemberExpression property, FilterItem item)
        {
            if (item.Value == null || !decimal.TryParse(item.Value.ToString(), out var numericValue))
                return null;

            var targetType = Nullable.GetUnderlyingType(property.Type) ?? property.Type;
            var convertedValue = Convert.ChangeType(numericValue, targetType);
            var constant = Expression.Constant(convertedValue, property.Type);

            return item.Operator switch
            {
                "=" => Expression.Equal(property, constant),
                ">" => Expression.GreaterThan(property, constant),
                "<" => Expression.LessThan(property, constant),
                ">=" => Expression.GreaterThanOrEqual(property, constant),
                "<=" => Expression.LessThanOrEqual(property, constant),
                _ => null
            };
        }

        private static Expression BuildDateExpression(MemberExpression property, FilterItem item)
        {
            if (item.Operator == "between" && item.Values != null)
            {
                if (DateTime.TryParse(item.Values.From?.ToString(), out var fromDate) &&
                    DateTime.TryParse(item.Values.To?.ToString(), out var toDate))
                {
                    var fromConst = Expression.Constant(fromDate, property.Type);
                    var toConst = Expression.Constant(toDate, property.Type);
                    var ge = Expression.GreaterThanOrEqual(property, fromConst);
                    var le = Expression.LessThanOrEqual(property, toConst);
                    return Expression.AndAlso(ge, le);
                }
            }
            else if (item.Value != null && DateTime.TryParse(item.Value.ToString(), out var dateVal))
            {
                var dateConst = Expression.Constant(dateVal, property.Type);
                
                var startOfDay = dateVal.Date;
                var nextDay = startOfDay.AddDays(1);

                var startConst = Expression.Constant(startOfDay, property.Type);
                var endConst = Expression.Constant(nextDay, property.Type);

                return item.Operator switch
                {
                    "=" => Expression.AndAlso(
                            Expression.GreaterThanOrEqual(property, startConst),
                            Expression.LessThan(property, endConst)
                            ),
                    ">" => Expression.GreaterThan(property, dateConst),
                    "<" => Expression.LessThan(property, dateConst),
                    ">=" => Expression.GreaterThanOrEqual(property, dateConst),
                    "<=" => Expression.LessThanOrEqual(property, dateConst),
                    _ => null
                };
            }

            return null;
        }

        private static Expression BuildBoolExpression(MemberExpression property, FilterItem item)
        {
            if (item.Value == null)
                return null;

            if (bool.TryParse(item.Value.ToString(), out var boolValue))
            {
                var constant = Expression.Constant(boolValue, typeof(bool));
                return Expression.Equal(property, constant);
            }

            return null;
        }

        public static IEnumerable<SqlParameter> BuildDateQuery(FilterItem item, string key, string paramName, ref List<string> whereClauses)
        {
            var parameters = new List<SqlParameter>();

            if (item.Operator?.Equals("Between", StringComparison.OrdinalIgnoreCase) == true)
            {
                if (DateTime.TryParse(item.Values?.From?.ToString(), out DateTime fromDate) &&
                    DateTime.TryParse(item.Values?.To?.ToString(), out DateTime toDate))
                {


                    var fromParam = $"{paramName}_From";
                    var toParam = $"{paramName}_To";

                    whereClauses.Add($"{key} BETWEEN {fromParam} AND {toParam}");
                    parameters.Add(new SqlParameter(fromParam, fromDate));
                    parameters.Add(new SqlParameter(toParam, toDate));
                }
                else
                    whereClauses.Add("1 = 0");
                
                return parameters;

            }

            if (item.Value == null)
            {
                whereClauses.Add($"{key} IS NULL");
                return parameters;
            }
            
            if (!DateTime.TryParse(item.Value.ToString(), out DateTime dateValue))
            {
                whereClauses.Add("1 = 0"); //return no result
                return parameters;
            }
            
            string op = item.Operator switch
            {
                ">" or "<" or ">=" or "<=" or "=" => item.Operator,
                _ => "="
            };

            parameters.Add(new SqlParameter(paramName, dateValue));
            whereClauses.Add($"{key} {op} {paramName}");

            return parameters;
        }

        public static IEnumerable<SqlParameter> BuildStringQuery(FilterItem item, string key, string paramName, ref List<string> whereClauses)
        {
            var parameters = new List<SqlParameter>();
            var value = item.Value?.ToString();

            if (value == null)
            {
                whereClauses.Add($"{key} IS NULL");
                parameters.Add(new SqlParameter(paramName, DBNull.Value));
                return parameters;
            }

            whereClauses.Add($"{key} LIKE {paramName}");
            parameters.Add(new SqlParameter(paramName, $"%{value}%"));
            return parameters;

        }

        public static IEnumerable<SqlParameter> BuildNumberQuery(FilterItem item, string key, string paramName, ref List<string> whereClauses)
        {
            var parameters = new List<SqlParameter>();

            if (item.Value == null|| !decimal.TryParse(item.Value.ToString(), out var numericValue))
            {
                whereClauses.Add($"{key} IS NULL");
                parameters.Add(new SqlParameter(paramName, DBNull.Value));
                return parameters;
            }

            string op = item.Operator switch
            {
                ">" or "<" or ">=" or "<=" or "=" => item.Operator,
                _ => "="
            };

            whereClauses.Add($"{key} {op} {paramName}");
            parameters.Add(new SqlParameter(paramName, numericValue));
            return parameters;
        }
    }
}
