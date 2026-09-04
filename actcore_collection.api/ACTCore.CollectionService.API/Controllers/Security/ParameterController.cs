using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Models.Dto.DisplayColumnDto;
using ACTCore.CollectionService.API.Models.Dto.ListPagedDto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Securities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using SharedKernel.Models;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class ParameterController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly ParameterService _sysParameterService;
        private readonly IActivityLogService _activityLog;
        private readonly LanguageService _languageService;

        public ParameterController(IMapper mapper, IErrorLogService errorLog,
            ParameterService sysParameterService,
            IActivityLogService activityLog, LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _sysParameterService = sysParameterService;
            _activityLog = activityLog;
            _languageService = languageService;
        }

        /// <summary>
        /// Get list data with pagination
        /// </summary>
        /// <returns></returns>
        [HttpPost("list-paged")]
        public async Task<ActionResult<APIResponse>> GetListParameters(PagedRequest<FilterContainer> request)
        {
            var response = new ApiPaginationResponse();
            try
            {
                var parameters = await _sysParameterService.GetListPaginationAsync(
                    filter: null,
                    pageSize: request.PageSize,
                    pageNumber: request.PageNumber,
                    sortColumn: request.SortColumn,
                    sortDirection: request.SortDirection,
                    filterColumns: request.Filters
                );

                List<SysParameterListPagedResponseDto> listPaged = new List<SysParameterListPagedResponseDto>();
                foreach (var item in parameters.Items)
                {
                    var parameter = new SysParameterListPagedResponseDto();
                    parameter.Id = item.Id;
                    parameter.ParameterCategory = item.ParameterCategory;
                    parameter.ParameterName = item.ParameterName;
                    parameter.ParameterValue = item.ParameterValue;
                    parameter.Description = item.Description;
                    parameter.IsSystem = item.IsSystem ? IconType.CHECKGREEN
                        : IconType.CHECKRED;
                    listPaged.Add(parameter);
                }

                var enumValues = await LangHelper.GetEnumBoolAsync(_languageService);
                var displayProperties = Utilities.DataTableHelper.GetDisplayColumnAndDisplayProperties<SysParameterDisplayFilterDto>(enumValues);

                response = new ApiPaginationResponse()
                {
                    Data = new PaginationData<SysParameterListPagedResponseDto>
                    {
                        Datatables = listPaged,
                        DisplayColumns = displayProperties,
                        TotalRecords = parameters.TotalCount,
                        TotalPages = (int)Math.Ceiling((double)parameters.TotalCount / request.PageSize),
                        PageNumber = request.PageNumber,
                        PageSize = request.PageSize,
                    },
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
                        LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response)
                        );
            }
        }

        [HttpGet("get-by-id/{id}")]
        public async Task<ActionResult<APIResponse>> GetById(int id)
        {
            var response = new APIResponse();
            try
            {
                var parameter = await _sysParameterService.GetByIdAsync(id);
                if (parameter == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    //response.Message = (await _languageService.GetResponseMultiLangMsgAsync(x => x.Key == ResponseMessage.Msg_NoDataFound))?.Value;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoDataFound);
                    response.Status = false;
                    return Ok(response);
                }

                response.Data = _mapper.Map<SysParameterResponseDto>(parameter);
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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, id.ToString(), ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateParameter([FromBody] SysParameterUpdateDto updateDto)

        {
            var response = new APIResponse();
            SysParameter updateParam = null;
            SysParameter beforUpdate = null;
            try
            {
                if (updateDto == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidData);
                    response.Status = false;
                    return Ok(response);
                }

                updateParam = await _sysParameterService.GetByIdAsync(updateDto.Id, asNoTracking: false);
                if (updateParam == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NoDataFound);
                    response.Status = false;
                    return Ok(response);
                }

                beforUpdate = Utilities.ModelHelper.CloneModel(updateParam);
                _mapper.Map(updateDto, updateParam);

                VersioningModelHelper.SetUpdatedAudit(updateParam, User.Identity.Name);
                await _sysParameterService.UpdateAsync(updateParam);

                response.StatusCode = HttpStatusCode.OK;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SavedSuccessfully);
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
                await _activityLog.ActivityLogEditWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.UPDATE, updateDto.Id.ToString(), ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updateParam)
                    );
            }
        }
    }
}