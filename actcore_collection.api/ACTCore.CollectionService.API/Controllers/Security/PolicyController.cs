using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
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
using System.Net;


namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class PolicyController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _logger;
        private readonly IActivityLogService _activityLog;
        private readonly PolicyService _policyService;
        private readonly UserService _userService;
        private readonly LanguageService _languageService;

        public PolicyController(IMapper mapper, IErrorLogService errorLog, IActivityLogService activityLog,
            PolicyService policyService, UserService userService, LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _logger = errorLog;
            _activityLog = activityLog;
            _policyService = policyService;
            _userService = userService;
            _languageService = languageService;
        }

        [HttpGet("get-list")]
        public async Task<ActionResult<APIResponse>> GetListSysPolicies()
        {
            var response = new APIResponse();
            try
            {
                var item = await _policyService.GetListAsync();
                var itemDto = _mapper.Map<List<SysPolicyResponseDto>>(item);

                response.Data = itemDto;
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
                    LoggerHelper.BuildActivityLog(ActivityLogAction.VIEW, "", ControllerContext, CurrentUserInfo, response)
                    );
            }
        }

        [HttpPost("update")]
        public async Task<ActionResult<APIResponse>> UpdateSysPolicy([FromBody] SysPolicyRequestDto updateDto)
        {
            var response = new APIResponse();
            SysPolicy updatePolicy = null;
            SysPolicy beforUpdate = null;
            try
            {
                if (updatePolicy == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Status = false;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Policy");
                    return Ok(response);
                }
                beforUpdate = Utilities.ModelHelper.CloneModel(updatePolicy);
                _mapper.Map(updateDto, updatePolicy);
                VersioningModelHelper.SetUpdatedAudit(updatePolicy, User.Identity.Name);

                await _policyService.UpdateAsync(updatePolicy);
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
                    LoggerHelper.BuildActiviUpdateLog(beforUpdate, updatePolicy)
                    );
            }
        }

    }

}
