using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using System.Net;


[Authorize]
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
public class ColAreaController : BaseApiController
{
    private readonly IMapper _mapper;
    private readonly IErrorLogService _errorLog;
    private readonly IActivityLogService _activityLog;
    private readonly ColAreaService _colAreaService;

    public ColAreaController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
        ColAreaService colAreaService) : base(errorLog)
    {
        _mapper = mapper;
        _errorLog = errorLog;
        _activityLog = activityLog;
        _colAreaService = colAreaService;
    }

    [HttpGet("list")]
    public async Task<ActionResult<APIResponse>> GetColAreaList()
    {
        var response = new APIResponse();
        try
        {
            var areas = await _colAreaService.GetListAsync(x => x.IsActive == true);

            response.Data = _mapper.Map<List<ColAreaListResponseDto>>(areas);
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
                LoggerHelper.BuildActiviLog(ActivityLogAction.VIEW, CurrentUserInfo.UserId.ToString(), ControllerContext, CurrentUserInfo, response)
            );
        }
    }
}