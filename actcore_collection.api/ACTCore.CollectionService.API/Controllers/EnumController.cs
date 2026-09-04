using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.API.Utilities;
using ACTCore.CollectionService.Application;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v{version:apiVersion}/[controller]")]
    public class EnumController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IActivityLogService _activityLogService;
        private readonly EnumService _enumService;
        private readonly LanguageService _languageService;
        private readonly IActivityLogService _activityLog;

        public EnumController(IMapper mapper, IErrorLogService errorLog,
            IActivityLogService activityLogService, EnumService enumService,
            LanguageService languageService) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _activityLogService = activityLogService;
            _enumService = enumService;
            _languageService = languageService;
        }

        [HttpGet("list-by-enum-name/{enumName}")]
        public async Task<ActionResult<APIResponse>> GetListUserGroups(string enumName)
        {
            var response = new APIResponse();
            try
            {
                var enums = await _enumService.GetListAsync(x => x.EnumName == enumName);
                if (!enums.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "Data");
                    response.Status = false;

                    return Ok(response);
                }

                response.Data = _mapper.Map<IEnumerable<EnumListResponseDto>>(enums.OrderBy(x => x.EnumOrder));
                response.StatusCode = HttpStatusCode.OK;
                response.Status = true;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.ErrorMessages.Add(ex.Message);
                return await HandleExceptionAsync(response, ex);
            }
        }
    }
}
