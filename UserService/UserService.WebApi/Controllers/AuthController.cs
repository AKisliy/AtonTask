using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using UserService.Core.Interfaces.Services;
using UserService.Infrastructure.Options;
using UserService.WebApi.Dto.Errors;
using UserService.WebApi.Dto.Request;

namespace UserService.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(IAuthService authService, IOptions<MyCookiesOptions> cookiesOptions) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly MyCookiesOptions _cookiesOptions = cookiesOptions.Value;

        /// <summary>
        /// Login with user's login and password (JWT is used)
        /// </summary>
        /// <param name="user">Body with login and password</param>
        /// <response code="200">Success</response>
        /// <response code="404">User with Login not found</response>
        /// <response code="403">Incorrect password or user was revoked</response>
        /// <response code="500">Server problems :(</response>
        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.NotFound)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.Forbidden)]
        [ProducesResponseType<ErrorResponse>((int)HttpStatusCode.InternalServerError)]
        public async Task<IActionResult> Login(UserLoginRequest user)
        {
            var token = await _authService.Login(user.Login, user.Password);
            HttpContext.Response.Cookies.Append(_cookiesOptions.TokenFieldName, token);
            return Ok();
        }
    }
}