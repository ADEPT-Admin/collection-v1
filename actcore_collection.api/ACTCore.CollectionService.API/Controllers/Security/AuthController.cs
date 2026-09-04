using ACTCore.CollectionService.API.Controllers.BaseController;
using ACTCore.CollectionService.API.Models;
using ACTCore.CollectionService.API.Models.Dto;
using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Application.Security;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.CommonConstants;
using System.Net;


namespace ACTCore.CollectionService.API.Controllers.Security
{
    [Route("api/v{version:apiVersion}/[controller]")]
    [ApiController]
    public class AuthController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly IErrorLogService _errorLog;
        private readonly IAuthLogService _authLog;
        private readonly ActiveSessionService _activeSessionService;
        private readonly UserService _userService;
        private readonly AuthService _authService;
        private readonly LanguageService _languageService;

        public AuthController(
            IMapper mapper,
            IErrorLogService errorLog,
            IAuthLogService authLog,
            ActiveSessionService activeSessionService,
            UserService userService,
            AuthService authService,
            LanguageService languageService
        ) : base(errorLog)
        {
            _mapper = mapper;
            _errorLog = errorLog;
            _authLog = authLog;
            _activeSessionService = activeSessionService;
            _userService = userService;
            _authService = authService;
            _languageService = languageService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            var sessionId = Guid.NewGuid();
            var response = new APIResponse();

            LoginResultDto loginResultDto = await _authService.Login(model.UserName, model.Password, sessionId);
            if (!loginResultDto.Response.IsSuccess)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Status = false;
                response.Message = loginResultDto.Response.Message;

                await _authLog.AddAuthLogAsync(loginResultDto.User?.UserId, model?.UserName, AuthLogStatus.LOGIN_FAILED,
                    response.Message?.En, sessionId);
                return Ok(response);
            }
            await _authLog.AddAuthLogAsync(loginResultDto.User.UserId, loginResultDto.User.UserName, AuthLogStatus.LOGIN_SUCCESS,
                null, sessionId);

            response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_LoginSuccessfully);
            response.StatusCode = HttpStatusCode.OK;
            response.Status = true;
            response.Data = _mapper.Map<TokenRequestDto>(loginResultDto.Response);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            var response = new APIResponse();

            await _authService.Logout(CurrentUserInfo.SessionId, CurrentUserInfo.UserId, CurrentUserInfo.TokenId);
            await _authLog.UpdateAuthLogAsync(CurrentUserInfo.SessionId, AuthLogStatus.LOGOUT);

            response.Message = await LangHelper.GetResponseMsgAsync(_languageService,
                    Message.Msg_LogoutSuccessfully);
            response.StatusCode = HttpStatusCode.OK;
            response.Status = true;
            response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_LogoutSuccessful);
            return Ok(response);
        }

        [Authorize]
        [HttpPost("session-expired")]
        public async Task<IActionResult> SessionExpired()
        {
            var response = new APIResponse();

            await _activeSessionService.RevokeSessionAsync(CurrentUserInfo.SessionId, "Session expired");
            await _authLog.UpdateAuthLogAsync(CurrentUserInfo.SessionId, AuthLogStatus.SESSION_EXPIRED);

            response.StatusCode = HttpStatusCode.OK;
            response.Status = true;
            response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_SessionExpiredandRevoked);
            return Ok(response);
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequestDto tokenDto)
        {
            var response = new APIResponse();

            if (!ModelState.IsValid)
            {
                response.Status = false;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidInput);
                return BadRequest(response);
            }

            var tokenResponse = await _authService.RefreshAccessToken(tokenDto.AccessToken, tokenDto.RefreshToken);
            if (tokenResponse.TokenDto == null || string.IsNullOrEmpty(tokenResponse.TokenDto.AccessToken))
            {
                response.Status = false;
                response.StatusCode = HttpStatusCode.BadRequest;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidRefreshToken);
                return BadRequest(response);
            }

            response.Status = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_TokenRefreshedSuccessfully);
            response.Data = tokenResponse;
            return Ok(response);
        }

        [Authorize]
        [HttpPost("revoke")]
        public async Task<IActionResult> RevokeRefreshToken([FromBody] TokenRequestDto tokenDto)
        {
            var response = new APIResponse();

            if (!ModelState.IsValid)
            {
                response.Status = false;
                response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_InvalidInput);
                return BadRequest(response);
            }

            await _authService.RevokeRefreshToken(tokenDto.AccessToken, tokenDto.RefreshToken);

            response.Status = true;
            response.StatusCode = HttpStatusCode.OK;
            response.Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_RefreshTokenRevokedSuccessfully);
            return Ok(response);
        }
    }
}
