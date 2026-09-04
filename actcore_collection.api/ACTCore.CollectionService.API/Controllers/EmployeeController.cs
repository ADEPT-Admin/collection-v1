using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Extensions;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Profiles;
using ACTCore.CollectionService.Domain.ValueObjects;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Models;
using System.Linq.Expressions;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EmployeeController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly EmployeeService _employeeService;
        private readonly LanguageService _languageService;

        public EmployeeController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            EmployeeService employeeService, LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _employeeService = employeeService;
            _languageService = languageService;
        }

        [HttpGet("list")]
        public async Task<ActionResult<APIResponse>> GetList()
        {
            var response = new APIResponse();
            try
            {
                var items = await _employeeService.GetListAsync(x => x.EmployeeId != "00000");
                var dtos = items.Select(item => new EmployeeResponseDto
                {
                    EmployeeId = item.EmployeeId,
                    UserName = item.UserName,
                    EmployeeName = item.FullName
                }).ToList();

                response.Data = dtos;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Status = false;
                response.Message = new LanguageValue() { En = ex.Message, Th = ex.Message };
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }

        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListEmployeesPaged([FromBody] PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var isDialogRequest = !String.IsNullOrEmpty(request.Mode) 
                    && String.Equals(request.Mode, ModeType.Dialog, StringComparison.OrdinalIgnoreCase);
                var filter = BuildExpressionFilter(request, isDialogRequest);
                
                request.SortColumn = request.SortColumn switch
                {
                    "EmployeeName" => "FullName",
                    "PositionName" => "Position.PositionName",
                    "DepartmentName" => "Department.DepartmentName",
                    _ => request.SortColumn
                };

                var employees = await _employeeService.GetListPaginationAsync(
                    filter: filter,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn ?? nameof(EmployeeProfile.EmployeeId),
                    sortDirection: request.SortDirection?? "asc",
                    filterColumns: request.Filters,
                    includeProperties: isDialogRequest ? "Prefix,Department,Position" : "Prefix"
                );

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                object data = isDialogRequest
                    ? PaginationDataHelper.BuildPaginationData<EmployeeDialogDisplayFilterDto, EmployeeDialogListPagedResponseDto, EmployeeProfile>
                        (employees, request.PageNumber, request.PageSize, enumValues, _mapper)
                    : PaginationDataHelper.BuildPaginationData<EmployeeDisplayFilterDto, EmployeeListPagedResponseDto, EmployeeProfile>
                        (employees, request.PageNumber, request.PageSize, enumValues, _mapper);

                response = new ApiPaginationResponse()
                {
                    Data = data,
                    StatusCode = HttpStatusCode.OK,
                    Status = true
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response));
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(string id)
        {
            var response = new APIResponse();
            try
            {
                var employee = await _employeeService.GetAsync(x => x.EmployeeId == id, includeProperties: "Prefix");
                if (employee == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Employee");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = _mapper.Map<EmployeeResponseDto>(employee); ;
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }
        private static Expression<Func<EmployeeProfile, bool>> BuildExpressionFilter(PagedRequest<FilterContainer> request, bool isDialogRequest = false)
        {
            Expression<Func<EmployeeProfile, bool>> filter = x => !isDialogRequest || x.IsActive == true;
            if (request.Filters?.DynamicFilters != null)
            {
                foreach (var dynamicFilter in request.Filters.DynamicFilters)
                {
                    string key = dynamicFilter.Key;
                    string value = dynamicFilter.Value?.returnStringValue();

                    if (value == null || string.IsNullOrWhiteSpace(value)) continue;
                    // EmployeeName
                    if (String.Equals(key, nameof(EmployeeListPagedResponseDto.EmployeeName), StringComparison.OrdinalIgnoreCase) 
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<EmployeeProfile, bool>> empFilter = u =>
                            u != null && (string.Join(" ",
                                new[] {u.Prefix != null ? u.Prefix.PrefixName : "",
                                u.FirstName ?? "",
                                u.LastName ?? "" }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // EmployeeNameEn
                    else if (String.Equals(key, nameof(EmployeeListPagedResponseDto.EmployeeNameEn), StringComparison.OrdinalIgnoreCase) 
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<EmployeeProfile, bool>> empFilter = u =>
                            u != null && (string.Join(" ",
                                new[] {u.Prefix != null ? u.Prefix.PrefixNameEn : "",
                                u.FirstNameEn ?? "",
                                u.LastNameEn ?? "" }
                                ).Contains(value)
                            );
                        filter = FilterExtensions.AndAlso(filter, empFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // Department
                    else if(String.Equals(key, nameof(EmployeeDialogListPagedResponseDto.DepartmentName), StringComparison.OrdinalIgnoreCase) 
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<EmployeeProfile, bool>> depFilter = d =>
                            d != null && d.Department != null && d.Department.DepartmentName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, depFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                    // PositionName
                    else if(String.Equals(key, nameof(EmployeeDialogListPagedResponseDto.PositionName), StringComparison.OrdinalIgnoreCase) 
                        && !string.IsNullOrWhiteSpace(value))
                    {
                        Expression<Func<EmployeeProfile, bool>> positionFilter = p =>
                            p != null && p.Position != null && p.Position.PositionName.Contains(value);
                        filter = FilterExtensions.AndAlso(filter, positionFilter);
                        request.Filters.DynamicFiltersRaw.Remove(key);
                    }
                }
            }

            return filter;
        }

        /*
        [NonAction]
        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> Create([FromBody] EmployeeProfileCreateDto createDto)
        {
            var response = new APIResponse();
            try
            {
                if (createDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Invalid data!";
                    response.Status = false;
                    return BadRequest(response);
                }

                var exists = await _employeeService.GetAsync(x => x.EmployeeId == createDto.EmployeeId);
                if (exists != null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "EmployeeId already exists!";
                    response.Status = false;
                    return BadRequest(response);
                }

                var entity = _mapper.Map<EmployeeProfile>(createDto);
                VersioningModelHelper.SetCreatedAudit(entity, User.Identity.Name);
                await _employeeService.AddAsync(entity);

                response.Data = _mapper.Map<EmployeeProfileDto>(entity);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Status = false;
                response.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }*/

        /*
        [NonAction]
        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> Update([FromBody] EmployeeProfileUpdateDto updateDto)
        {
            var response = new APIResponse();
            try
            {
                if (updateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = "Invalid data!";
                    response.Status = false;
                    return BadRequest(response);
                }

                var entity = await _employeeService.GetAsync(x => x.EmployeeId == updateDto.EmployeeId, asNoTracking: false);
                if (entity == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = "No data found.";
                    response.Status = false;
                    return Ok(response);
                }

                _mapper.Map(updateDto, entity);
                VersioningModelHelper.SetUpdatedAudit(entity, User.Identity.Name);

                await _employeeService.UpdateAsync(entity);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Status = false;
                response.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
        */

        /*
        [NonAction]
        [HttpPost("delete")]
        public async Task<ActionResult<APIResponse>> Delete([FromBody] EmployeeProfileIdRequestDto request)
        {
            var response = new APIResponse();
            try
            {
                var entity = await _employeeService.GetAsync(x => x.EmployeeId == request.EmployeeId);
                if (entity == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    return Ok(response);
                }

                await _employeeService.DeleteAsync(entity);

                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Status = false;
                response.Message = ex.Message;
                return StatusCode((int)HttpStatusCode.InternalServerError, response);
            }
        }
        */
    }
}