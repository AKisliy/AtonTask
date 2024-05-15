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

        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.NotFound)]
        public async Task<IActionResult> Login(UserLoginRequest user)
        {
            var token = await _authService.Login(user.Login, user.Password);
            HttpContext.Response.Cookies.Append(_cookiesOptions.TokenFieldName, token);
            return Ok();
        }
    }
}