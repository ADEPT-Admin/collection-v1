using ACTCore.CollectionService.Application.Dto;
using ACTCore.CollectionService.Application.Helpers;
using ACTCore.CollectionService.Application.Interface;
using ACTCore.CollectionService.Domain.Entities.Securities;
using ACTCore.CollectionService.Infrastructure.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SharedKernel.CommonConstants;
using SharedKernel.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;


namespace ACTCore.CollectionService.Application.Security
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        private readonly IConfiguration _configuration;
        private readonly UserService _userService;
        private readonly PolicyService _policyService;
        private readonly ActiveSessionService _activeSessionService;
        private readonly IAuthLogService _authLogService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly LanguageService _languageService;
        private string secretKey;
        private readonly PasswordHasher<SysUser> _passwordHasher = new();

        public AuthService(IUnitOfWork unitOfWork, IMapper mapper, IConfiguration configuration,
                    UserService userService, PolicyService policyService,
                    ActiveSessionService activeSessionService, IAuthLogService authLogService,
                    IHttpContextAccessor httpContextAccessor, LanguageService languageService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _configuration = configuration;
            _userService = userService;
            _policyService = policyService;
            _activeSessionService = activeSessionService;
            _authLogService = authLogService;
            _httpContextAccessor = httpContextAccessor;
            _languageService = languageService;
            secretKey = _configuration.GetSection("ApiSettings:Secret").Value;
        }

        public async Task<LoginResultDto> Login(string userName, string password, Guid sessionId)
        {
            LoginResponseDto loginResponse = new();
            var user = await _unitOfWork.Repository<SysUser>().GetAsync(x => x.UserName.ToLower() == userName.ToLower(),
                includeProperties: "UserGroupAccesses.UserGroup", asNoTracking: false);
            if (user == null)
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto { IsSuccess = false, Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_NotFound, "User") }
                };

            if (!user.IsActive)
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto { IsSuccess = false, Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccountSuspended) }
                };

            // check usergroup inactive
            if (!user.UserGroupAccesses.Any(uga => uga.IsActive && (uga.UserGroup?.IsActive ?? false)))
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccessDenied)
                    }
                };

            if (user.IsLockUser)
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto { IsSuccess = false, Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccountLocked) }
                };

            if (user.EffectiveDate == null)
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccountEffectiveDateNotSet)
                    }
                };
            if (user.EffectiveDate > DateTime.Now)
            {
                var message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccountEffectiveOn, user.EffectiveDate?.ToString("dd/MM/yyyy"));
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = message
                    }
                };
            }

            if (user.ExpireDate != null && user.ExpireDate < DateTime.Now)
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto { IsSuccess = false, Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_UserAccountExpired) }
                };

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            if (result != PasswordVerificationResult.Success)
            {
                var lockedPolicy = await _policyService.GetAsync(x => x.PolicyCode == "locked_user");
                int pilicyHit = (lockedPolicy != null && !string.IsNullOrEmpty(lockedPolicy.PolicyValue)) ? int.Parse(lockedPolicy.PolicyValue) : 3;

                int missignHit = user.MissingHit.HasValue ? user.MissingHit.Value + 1 : 1;
                if (missignHit == pilicyHit)
                {
                    user.MissingHit = missignHit;
                    user.IsLockUser = true;
                    await _userService.UpdateAsync(user);
                    VersioningModelHelper.SetUpdatedAudit(user, user.UserName);
                    return new LoginResultDto
                    {
                        User = user,
                        Response = new LoginResponseDto
                        {
                            IsSuccess = false,
                            Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordIncorrectMissingLocked, user.MissingHit)
                        }
                    };
                }

                user.MissingHit = missignHit;
                await _userService.UpdateAsync(user);
                VersioningModelHelper.SetUpdatedAudit(user, user.UserName);
                return new LoginResultDto
                {
                    User = user,
                    Response = new LoginResponseDto
                    {
                        IsSuccess = false,
                        Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_PasswordIncorrectMissing, user.MissingHit)
                    }
                };
            }

            user.LastSignOnDate = DateTime.Now;
            user.MissingHit = 0;
            VersioningModelHelper.SetUpdatedAudit(user, user.UserName);
            await _userService.UpdateAsync(user);

            var jwtTokenId = $"JTI{Guid.NewGuid()}";
            var accessToken = GetAccessToken(user, jwtTokenId, sessionId);

            var ipAddress = _httpContextAccessor.HttpContext.Connection.RemoteIpAddress?.ToString();
            var userAgent = _httpContextAccessor.HttpContext?.Request.Headers["User-Agent"].ToString();
            var deviceInfo = DeviceHelper.ParseDeviceInfo(userAgent);
            var location = await DeviceHelper.GetLocationFromIPAsync(ipAddress);


            await _activeSessionService.CreateSessionAsync(sessionId, user.UserId, user.UserName, ipAddress, userAgent, deviceInfo, location);

            var refreshToken = await CreateNewRefreshToken(user.UserId, jwtTokenId, sessionId);
            var refreshHash = HashHelper.ComputeHash(refreshToken);
            await _activeSessionService.UpdateRefreshTokenHashAsync(sessionId, refreshHash);

            return new LoginResultDto
            {
                User = user,
                Response = new LoginResponseDto
                {
                    IsSuccess = true,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                    Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_LoginSuccessful)
                }
            };
        }

        public async Task Logout(Guid sessionId, Guid userId, string tokenId)
        {
            await _activeSessionService.RevokeSessionAsync(sessionId, "User logout");
            await MarkAllTokenInChainAsInValid(userId, tokenId);
        }

        private string GetAccessToken(SysUser user, string jwtTokenId, Guid sessionId)
        {
            var roles = user.UserGroupAccesses.Select(g => g.UserGroup.UserGroupName).ToList();

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString().ToLower()),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, jwtTokenId),
                new Claim("empid", user.EmployeeId ?? "none"),
                new Claim("sessionid", sessionId.ToString()),
            };
            // Add each role as a separate claim
            claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

            var key = Encoding.UTF8.GetBytes(secretKey);
            double tokenExpires = Convert.ToDouble(_configuration["ApiSettings:AccessTokenExpiresMinutes"]);
            var issuer = _configuration["ApiSettings:Issuer"];
            var audience = _configuration["ApiSettings:Audience"];

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                // Use UtcNow to avoid timezone issues
                Expires = DateTime.UtcNow.AddMinutes(tokenExpires),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenStr = tokenHandler.WriteToken(token);
            return tokenStr;
        }

        public async Task<TokenResultDto> RefreshAccessToken(string accessToken, string refreshToken)
        {
            // Find an existing refresh token
            var refreshHash = HashHelper.ComputeHash(refreshToken);
            var existingRefreshToken = await _unitOfWork.Repository<RefreshToken>()
                .GetAsync(x => x.RefreshTokenHash == refreshHash, asNoTracking: false);

            if (existingRefreshToken == null)
                return new TokenResultDto();

            // Check if the token has been used.
            if (existingRefreshToken.IsUsed)
            {
                // An attempt was made to reuse the refresh token.
                var activeSession = await _unitOfWork.Repository<ActiveSession>()
                    .GetAsync(x => x.SessionId == existingRefreshToken.SessionId, asNoTracking: false);
                if (activeSession != null)
                {
                    activeSession.ReuseDetected = true;
                    activeSession.IsRevoked = true;
                    await _unitOfWork.SaveChangesAsync();
                }

                await MarkAllTokenInChainAsInValid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
                return new TokenResultDto { Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_RefreshTokenReuseDetected), TokenDto = new TokenDto() };
            }

            // Check if the token is still valid. When someone tries to use not valid refresh token, fruad posible
            if (!existingRefreshToken.IsValid)
            {
                await MarkAllTokenInChainAsInValid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
                return new TokenResultDto();
            }

            // If just expired then mark as invalid and return empty (Use UtcNow to avoid timezone issues)
            if (existingRefreshToken.ExpireAt < DateTime.UtcNow)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenResultDto();
            }

            // Check if the session is still active or not.
            var activeSessionCheck = await _unitOfWork.Repository<ActiveSession>()
                .GetAsync(x => x.SessionId == existingRefreshToken.SessionId);
            if (activeSessionCheck == null || !activeSessionCheck.IsActive)
            {
                existingRefreshToken.IsValid = false;
                await _unitOfWork.SaveChangesAsync();

                await _activeSessionService.RevokeSessionAsync(existingRefreshToken.SessionId, "Session expired");
                await _authLogService.UpdateAuthLogAsync(existingRefreshToken.SessionId, AuthLogStatus.SESSION_EXPIRED);
                return new TokenResultDto();
            }

            // Compare data from existing refresh and access token privided and if there is any missmatch then consider it as a fraud
            var isTokenValid = GetAccessTokenData(accessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                await MarkTokenAsInvalid(existingRefreshToken);
                return new TokenResultDto();
            }

            // If everything is correct, this token is only to be used once.
            existingRefreshToken.IsUsed = true;
            existingRefreshToken.UsedAt = DateTime.UtcNow;
            await _unitOfWork.SaveChangesAsync();

            var sessionId = Guid.Parse(JwtHelper.GetValueFromToken(accessToken, "sessionid"));
            // replace old refresh with a new one with updated expiry date
            var newRefreshToken = await CreateNewRefreshToken(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId, sessionId);

            // revoke existing refresh token
            await MarkTokenAsInvalid(existingRefreshToken);

            // genreate new access token
            var applicationUser = await _unitOfWork.Repository<SysUser>().GetAsync(x => x.UserId == existingRefreshToken.UserId);
            if (applicationUser == null)
                return new TokenResultDto();

            var newAccessToken = GetAccessToken(applicationUser, existingRefreshToken.JwtTokenId, sessionId);

            return new TokenResultDto
            {
                Message = await LangHelper.GetResponseMsgAsync(_languageService, Message.Msg_TokenRefreshedSuccessfully),
                TokenDto = new TokenDto
                {
                    AccessToken = newAccessToken,
                    RefreshToken = newRefreshToken
                }
            };
        }

        public async Task RevokeRefreshToken(string accessToken, string refreshToken)
        {
            var existingRefreshToken = await _unitOfWork.Repository<RefreshToken>().GetAsync(x => x.RefreshTokenHash == refreshToken);

            if (existingRefreshToken == null)
                return;

            // Compare data from existing refresh and access token privided and if there is any missmatch then we should do nothing with refesh token
            var isTokenValid = GetAccessTokenData(accessToken, existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
            if (!isTokenValid)
            {
                return;
            }

            await MarkAllTokenInChainAsInValid(existingRefreshToken.UserId, existingRefreshToken.JwtTokenId);
        }

        private async Task<string> CreateNewRefreshToken(Guid userId, string jwtTokenId, Guid sessionId)
        {
            double refreshTokenExpires = Convert.ToDouble(_configuration["ApiSettings:RefreshTokenExpiresDays"]);

            var rawToken = Guid.NewGuid().ToString("N") + "-" + Guid.NewGuid().ToString("N");
            var tokenHash = HashHelper.ComputeHash(rawToken);

            var refreshToken = new RefreshToken()
            {
                SessionId = sessionId,
                UserId = userId,
                JwtTokenId = jwtTokenId,
                RefreshTokenHash = tokenHash,
                // Use UtcNow to avoid timezone issues 
                ExpireAt = DateTime.UtcNow.AddDays(refreshTokenExpires),
                IsValid = true,
                IsUsed = false,
                IssuedAt = DateTime.UtcNow
            };

            await _unitOfWork.Repository<RefreshToken>().AddAsync(refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return rawToken;
        }

        private bool GetAccessTokenData(string accessToken, Guid expectedUserId, string expectedTokenId)
        {
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwt = tokenHandler.ReadJwtToken(accessToken);
                var jwtTokenId = jwt.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;
                var userId = jwt.Claims.FirstOrDefault(x => x.Type == "nameid")?.Value;
                return userId == expectedUserId.ToString().ToLower() && jwtTokenId == expectedTokenId;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task MarkAllTokenInChainAsInValid(Guid userId, string tokenId)
        {
            var refreshTokens = await _unitOfWork.Repository<RefreshToken>()
                .GetAllAsync(x => x.UserId == userId && x.JwtTokenId == tokenId, asNoTracking: false);

            foreach (var refreshToken in refreshTokens)
            {
                refreshToken.IsValid = false;
            }

            await _unitOfWork.SaveChangesAsync();
        }

        private Task MarkTokenAsInvalid(RefreshToken refreshToken)
        {
            refreshToken.IsValid = false;
            return _unitOfWork.SaveChangesAsync();
        }

        public async Task MarkTokensInvalidBySessionId(Guid sessionId)
        {
            var refreshTokens = await _unitOfWork.Repository<RefreshToken>()
                .GetAllAsync(x => x.SessionId == sessionId && x.IsValid, asNoTracking: false);

            foreach (var token in refreshTokens)
            {
                token.IsValid = false;
            }

            await _unitOfWork.SaveChangesAsync();
        }
    }
}
