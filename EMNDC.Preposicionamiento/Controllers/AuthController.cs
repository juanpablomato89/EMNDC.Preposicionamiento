using AutoMapper;
using EMNDC.Preposicionamiento.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using EMNDC.Preposicionamiento.BasicResponses;
using EMNDC.Preposicionamiento.IServices;
using EMNDC.Preposicionamiento.Models;
using EMNDC.Preposicionamiento.Models.Requests;
using EMNDC.Preposicionamiento.Models.Responses;

namespace EMNDC.Preposicionamiento.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;
        private readonly IAuditService _audit;

        public AuthController(IAuthService authService, ITokenService tokenService, IMapper mapper, IAuditService audit)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(_authService));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
            _audit = audit;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register([FromBody] RegisterUserRequests request)
        {
            var user = await _authService.SignUpAsync(request);            
            var token = await _tokenService.GenerateTokens(user);
            return Ok(token);
        }
        [HttpPost("login")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(BasicUserResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequest), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login([FromBody] LoginRequests request)
        {
            try
            {
                var user = await _authService.LoginAsync(request);
                var token = await _tokenService.GenerateTokens(user);

                HttpContext.Response.Headers["Authorization"] = "Bearer " + token.AccessToken;
                HttpContext.Response.Headers["RefreshToken"] = token.RefreshToken;
                HttpContext.Response.Headers["Access-Control-Expose-Headers"] = "Authorization, RefreshToken";
                var mapper = _mapper.Map<BasicUserResponse>(user);

                await _audit.LogAsync(AuditActions.LoginSuccess, success: true,
                    userId: user.Id, userEmail: user.Email,
                    description: $"Login exitoso para {user.Email}");

                return Ok(new ApiOkResponse(mapper));
            }
            catch (Exception ex)
            {
                await _audit.LogAsync(AuditActions.LoginFail, success: false,
                    userEmail: request?.Email,
                    description: $"Login fallido: {ex.Message}");
                throw;
            }
        }

        [HttpPost("logout")]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequest), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Logout()
        {
            var userid = this.User.GetUserIdFromToken();
            await _tokenService.RevokeRefreshToken(userid);
            await _audit.LogAsync(AuditActions.Logout, userId: userid, description: "Logout");
            return Ok("Session closed successfully.");
        }

        [HttpPost("refresh-token")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(BadRequest), (int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequest request)
        {
            var newTokens = await _tokenService.RenewAccessToken(request.RefreshToken);
            if (newTokens == null)
                return Unauthorized("Invalid or expired Refresh Token.");

            return Ok(newTokens);
        }

        [HttpPost("send-code")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.RequestTimeout)]
        public async Task<IActionResult> SendResetCodeAsync([FromBody] SendEmailRequest email)
        {
            var sendCode = await _authService.SendResetCodeAsync(email);
            return Ok(new ApiOkResponse(sendCode));
        }

        [HttpPost("reset-password")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.RequestTimeout)]
        public async Task<IActionResult> ResetPasswordAsync([FromBody] ResetPasswordRequest request)
        {
            var result = await _authService.ResetPasswordAsync(request);
            return Ok(new ApiOkResponse(result));
        }

        [HttpGet("user-details")]
        [ProducesResponseType(typeof(ApiOkResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ApiResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> GetUserInformation()
        {
            var userId = this.User.GetUserIdFromToken();
            var user = await _authService.GetUserDetailsAsync(userId);
            var mapper = _mapper.Map<BasicUserResponse>(user);
            return Ok(new ApiOkResponse(mapper));
        }
    }
}
