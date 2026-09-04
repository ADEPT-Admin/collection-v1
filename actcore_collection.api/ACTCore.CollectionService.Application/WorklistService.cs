using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.ViewModel;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using EFCore.BulkExtensions;
using Microsoft.Data.SqlClient;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Linq.Expressions;
using System.Text;

namespace ACTCore.CollectionService.Application
{
    public class WorklistService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public WorklistService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Worklist> AddAsync(Worklist entity)
        {
            await _unitOfWork.Repository<Worklist>().AddAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(Worklist entity)
        {
            await _unitOfWork.Repository<Worklist>().DeleteAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Worklist>> GetListAsync(Expression<Func<Worklist, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Worklist>().GetAllAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<PagedResult<Worklist>> GetListPaginationAsync(Expression<Func<Worklist, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<Worklist>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<PagedResult<vw_ReassignWorklist>> GetListPaginationReassignWorklistAsync(Expression<Func<vw_ReassignWorklist, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<vw_ReassignWorklist>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<PagedResult<vw_UnassignWorklist>> GetListPaginationUnassignWorklistAsync(Expression<Func<vw_UnassignWorklist, bool>> filter = null,
            int pageSize = 0, int pageNumber = 1, string sortColumn = "", string sortDirection = "asc", FilterContainer filterColumns = null,
            string includeProperties = "", bool asNoTracking = true)
        {
            var repository = _unitOfWork.Repository<vw_UnassignWorklist>();

            var items = await repository.GetAllPagedAsync(filter,
                pageSize: pageSize, pageNumber: pageNumber,
                sortColumn: sortColumn, sortDirection: sortDirection, filterColumns: filterColumns,
                includeProperties: includeProperties, asNoTracking: asNoTracking);

            return items;
        }

        public async Task<PagedResult<WorklistListringDto>> GetListPaginationByViewAsync(ColTeamAssignment teamAssignment,
            string sortColumn = "", string sortDirection = "asc",
            int pageSize = 0, int pageNumber = 1,
            FilterContainer filterColumns = null)
        {
            string baseSql = "SELECT * FROM vw_WorklistListing";
            var repository = _unitOfWork.Repository<WorklistListringDto>();
            var sqlBuilder = new StringBuilder(baseSql);

            // Build WHERE clause and parameters
            var whereClauses = new List<string>();
            var sqlParams = new List<SqlParameter>();
            var sqlParamsTotalCount = new List<SqlParameter>();

            // Existing filterColumns logic...
            if (filterColumns?.DynamicFilters?.Any() == true)
            {
                foreach (var kvp in filterColumns?.DynamicFilters)
                {
                    var item = kvp.Value;
                    if (item == null) continue;

                    var paramName = $"@{kvp.Key}";
                    var parameters = Enumerable.Empty<SqlParameter>();

                    switch (item?.Type?.ToUpper())
                    {
                        case FilterType.INTEGER:
                        case FilterType.DECIMAL:
                            parameters = FilterHelper.BuildNumberQuery(item, kvp.Key, paramName, ref whereClauses);
                            break;

                        case FilterType.DATE:
                        case FilterType.DATETIME:
                            parameters = FilterHelper.BuildDateQuery(item, kvp.Key, paramName, ref whereClauses);
                            break;

                        case FilterType.STRING:
                        default:
                            parameters = FilterHelper.BuildStringQuery(item, kvp.Key, paramName, ref whereClauses);
                            break;
                    }

                    foreach (var p in parameters)
                    {
                        sqlParams.Add(p);
                        sqlParamsTotalCount.Add(new SqlParameter(p.ParameterName, p.Value ?? DBNull.Value));
                    }
                }
            }

            // Supervisor or Collector filter
            if (teamAssignment.IsSupervisor)
            {
                sqlBuilder.Append(" WHERE AssignTeamId ='" + teamAssignment.ColTeamId + "' ");
            }
            else
            {
                sqlBuilder.Append(" WHERE AssignCollectorId ='" + teamAssignment.CollectorId + "' ");
            }

            if (whereClauses.Count > 0)
            {
                sqlBuilder.Append(" AND " + string.Join(" AND ", whereClauses));
            }

            // Add ORDER BY
            if (!string.IsNullOrEmpty(sortColumn))
            {
                string[] splitSortColumn = sortColumn.Split(',');
                sortColumn = string.Join(",", splitSortColumn.Select(col => $"{col.Trim()} {sortDirection}"));
                sqlBuilder.Append($" ORDER BY {sortColumn}");
            }
            else
            {
                sqlBuilder.Append($" ORDER BY 1");
            }

            // Add paging
            if (pageSize > 0)
            {
                int offset = (pageNumber - 1) * pageSize;
                sqlBuilder.Append($" OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY");
            }

            // Query for items
            var itemsRaw = await repository.ExecuteSqlQueryAsync(sqlBuilder.ToString(), sqlParams.ToArray());

            var items = itemsRaw.Select(row => new WorklistListringDto
            {
                WorklistId = row.ContainsKey("WorklistId") && row["WorklistId"] != null ? (Guid)row["WorklistId"] : Guid.Empty,
                ContractNo = row.ContainsKey("ContractNo") ? row["ContractNo"]?.ToString() : null,
                CustomerName = row.ContainsKey("CustomerName") ? row["CustomerName"]?.ToString() : null,
                AssetType = row.ContainsKey("AssetType") ? row["AssetType"]?.ToString() : null,
                ContractStatus = row.ContainsKey("ContractStatus") ? row["ContractStatus"]?.ToString() : null,
                Bucket = row.ContainsKey("Bucket") && row["Bucket"] != null ? (int?)row["Bucket"] : null,
                DueDate = row.ContainsKey("DueDate") && row["DueDate"] != null ? (DateTime?)row["DueDate"] : null,
                DayPastDue = row.ContainsKey("DayPastDue") && row["DayPastDue"] != null ? (int?)row["DayPastDue"] : null,
                OverdueAmount = row.ContainsKey("OverdueAmount") && row["OverdueAmount"] != null ? (decimal?)row["OverdueAmount"] : null,
                OutstandingBalance = row.ContainsKey("OutstandingBalance") && row["OutstandingBalance"] != null ? (decimal?)row["OutstandingBalance"] : null,
                CollectorName = row.ContainsKey("CollectorName") ? row["CollectorName"]?.ToString() : null,
                ReassignBy = row.ContainsKey("ReassignBy") ? row["ReassignBy"]?.ToString() : null,
                ReassignFrom = row.ContainsKey("ReassignFrom") ? row["ReassignFrom"]?.ToString() : null,
                AssignDate = row.ContainsKey("AssignDate") && row["AssignDate"] != null ? (DateTime?)row["AssignDate"] : null,
                FollowupStatus = row.ContainsKey("FollowupStatus") ? row["FollowupStatus"]?.ToString() : null,
                LastFollowupDate = row.ContainsKey("LastFollowupDate") && row["LastFollowupDate"] != null ? (DateTime?)row["LastFollowupDate"] : null,
                NextFollowupDate = row.ContainsKey("NextFollowupDate") && row["NextFollowupDate"] != null ? (DateTime?)row["NextFollowupDate"] : null,
                PromiseToPayDate = row.ContainsKey("PromiseToPayDate") && row["PromiseToPayDate"] != null ? (DateTime?)row["PromiseToPayDate"] : null
            }).ToList();

            // Build the main SQL for itemsRaw
            var mainSql = sqlBuilder.ToString();

            // Remove ORDER BY and paging for count query
            var countSqlText = mainSql;
            int orderByIndex = countSqlText.IndexOf("ORDER BY", StringComparison.OrdinalIgnoreCase);
            if (orderByIndex >= 0)
                countSqlText = countSqlText.Substring(0, orderByIndex);

            int offsetIndex = countSqlText.IndexOf("OFFSET", StringComparison.OrdinalIgnoreCase);
            if (offsetIndex >= 0)
                countSqlText = countSqlText.Substring(0, offsetIndex);

            // Wrap with count
            var countSql = new StringBuilder();
            countSql.Append("SELECT COUNT(1) FROM (");
            countSql.Append(countSqlText);
            countSql.Append(") AS CountTable");

            // Use the same parameters as itemsRaw
            var totalCountRaw = await repository.ExecuteSqlQueryAsync(countSql.ToString(), sqlParamsTotalCount.ToArray());
            int totalCount = totalCountRaw.Count > 0 && totalCountRaw[0].Values.First() != null
                ? Convert.ToInt32(totalCountRaw[0].Values.First())
                : 0;

            return new PagedResult<WorklistListringDto>
            {
                Items = items,
                TotalCount = totalCount
            };
        }

        public async Task<Worklist> GetAsync(Expression<Func<Worklist, bool>> filter = null, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Worklist>().GetAsync(filter, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<Worklist> GetByIdAsync(Guid id, string includeProperties = "", bool asNoTracking = true)
        {
            return await _unitOfWork.Repository<Worklist>().GetAsync(x => x.WorklistId == id, includeProperties: includeProperties, asNoTracking: asNoTracking);
        }

        public async Task<Worklist> UpdateAsync(Worklist entity)
        {
            await _unitOfWork.Repository<Worklist>().UpdateAsync(entity);
            await _unitOfWork.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> BulkUpdateAsync(IEnumerable<Worklist> entities, BulkConfig bulkConfig = null)
        {
            await _unitOfWork.Repository<Worklist>().BulkUpdateAsync(entities, bulkConfig);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteRangeAsync(IEnumerable<Worklist> entities)
        {
            await _unitOfWork.Repository<Worklist>().DeleteRangeAsync(entities);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
    }
}
