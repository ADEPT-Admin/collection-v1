using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Collection;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using ACTCore.CollectionService.Domain.Entities.Collections;
using ACTCore.CollectionService.Domain.Entities.Contracts;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class CollectionNoteController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLog;
        private readonly WorklistService _worklistService;
        private readonly CollectionNoteService _collectionNoteService;
        private readonly TeamAssignmentService _teamAssignmentService;
        private readonly ColNoteActionService _colNoteActionService;
        private readonly ColNoteResultService _colNoteResultService;
        private readonly EnumService _enumService;
        private readonly LanguageService _languageService;
        private readonly ContractService _contractService;

        public CollectionNoteController(IMapper mapper, IErrorLogService errorLog,
            IActivityLogService activityLog, WorklistService worklistService, CollectionNoteService collectionNoteService, TeamAssignmentService teamAssignmentService,
            ColNoteActionService colNoteActionService, ColNoteResultService colNoteResultService,
            EnumService enumService, LanguageService languageService, ContractService contractService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLog = activityLog;
            _worklistService = worklistService;
            _collectionNoteService = collectionNoteService;
            _teamAssignmentService = teamAssignmentService;
            _colNoteActionService = colNoteActionService;
            _colNoteResultService = colNoteResultService;
            _enumService = enumService;
            _languageService = languageService;
            _contractService = contractService;
        }

        [HttpPost("create")]
        public async Task<ActionResult<APIResponse>> Create([FromBody] CollectionNoteCreateDto createDto)
        {
            var response = new APIResponse();
            var entityId = "";
            try
            {
                var workList = await _worklistService.GetByIdAsync(createDto.WorklistId, includeProperties: "Collector.User");
                if (workList == null || workList.ContractNo != createDto.ContractNo)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Worklist or ContractNo");
                    response.Status = false;
                    return Ok(response);
                }

                var hasPermission = await WorklistPermissionHelpers.CheckCollectorPermission(workList, CurrentUserInfo.UserId, _teamAssignmentService);
                if (!hasPermission)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotHavePermissionToAccessContract);
                    response.Status = false;
                    return Ok(response);
                }

                var colAction = await _colNoteActionService.GetAsync(x => x.ActionId == createDto.FollowupActionId);
                if (colAction == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Follow-up Action");
                    response.Status = false;
                    return Ok(response);
                }

                var colNoteResult = await _colNoteResultService.GetAsync(
                    x => x.ResultId == createDto.FollowupResultId 
                    && x.ActionId == createDto.FollowupActionId
                );
                if (colNoteResult == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Follow-up Result");
                    response.Status = false;
                    return Ok(response);
                }

                var followupStatusEnum = await _enumService.GetAsync(x => x.EnumName == "FollowupStatus" && x.EnumCode == colNoteResult.FollowupStatus);
                if (followupStatusEnum == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Enum FollowupStatus");
                    response.Status = false;
                    return Ok(response);
                }

                // find ContractPersonFullName, ContractPhoneNumber
                var person = await _contractService.GetContractPersonAsync(x =>
                    x.Id == createDto.ContractPersonId,
                    includeProperties: "Prefix");

                var phone = await _contractService.GetContractPhoneAsync(x => x.Id == createDto.ContractPhoneId);

                if (person == null || phone == null)
                { 
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService
                        , Message.Msg_NotFound
                        , (person == null) ? nameof(ContractPerson) : nameof(ContractPhone));
                    response.Status = false;
                    return Ok(response);
                }

                // Assign Value
                DateTime dateTime = DateTime.Now;
                var note = _mapper.Map<CollectionNote>(createDto);

                note.FollowupDate = dateTime;
                note.CreatedDate = dateTime;
                note.UpdatedDate = dateTime;
                note.ContractPersonFullName = person.FullName;
                note.ContractPhoneNumber = phone.PhoneNo;

                VersioningModelHelper.SetCreatedAudit(note, User.Identity.Name);
                await _collectionNoteService.AddAsync(note);

                
                workList.FollowupStatusCode = followupStatusEnum.EnumCode;
                workList.FollowupStatusDesc = followupStatusEnum.EnumDescription;
                workList.FollowupDate = dateTime;
                VersioningModelHelper.SetUpdatedAudit(workList, User.Identity.Name);
                await _worklistService.UpdateAsync(workList);


                entityId = note.Id.ToString();
                response.Data = _mapper.Map<CollectionNoteResponseDto>(note);
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_SavedSuccessfully);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
            finally
            {
                await _activityLog.ActivityLogCreateWithDetailsAsync(
                    LoggerHelper.BuildActivityLog(ActivityLogAction.CREATE, entityId, ControllerContext, CurrentUserInfo, response),
                    LoggerHelper.BuildActivityCreateLog(createDto)
                    );
            }
        }

    }
}
