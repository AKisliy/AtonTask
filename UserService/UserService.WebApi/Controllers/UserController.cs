using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Interfaces.Services;
using UserService.Core.Models;
using UserService.WebApi.Dto.Request;
using UserService.WebApi.Dto.Response;
using UserService.WebApi.Extensions;

namespace UserService.WebApi.Controllers
{
    [ApiController]
    [Route("api/user")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;

        public UserController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }

        [Authorize("Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserRegisterRequest request)
        {
            var creator = HttpContext.GetUserLogin();
            var id = await _userService.CreateUser(_mapper.Map<User>(request), creator);
            return Created($"api/user/{id}", new {UserId = id});
        }

        [Authorize("Admin")]
        [HttpGet("all/active")]
        public IActionResult GetActiveUsers()
        {
            var users = _userService.GetActiveUsers();
            return Ok(users.Select(u => _mapper.Map<UserResponse>(u)));
        }

        [Authorize("Admin")]
        [HttpGet("{login}")]
        public async Task<IActionResult> GetUserByLogin(string login)
        {
            var user = await _userService.GetUserByLogin(login);
            return Ok(_mapper.Map<UserByLoginResponse>(user));
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var id = HttpContext.GetUserId();
            var user = await _userService.GetUserById(id);
            return Ok(_mapper.Map<UserResponse>(user));
        }

        [Authorize("Admin")]
        [HttpGet("all/{age}")]
        public IActionResult GetUsersOlderThan(int age)
        {
            var users = _userService.GetUsersOlderThan(age);
            return Ok(users.Select(u => _mapper.Map<UserResponse>(u)));
        }

        [Authorize("Admin")]
        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteUser(string login, bool hard)
        {
            var revokerLogin = HttpContext.GetUserLogin();
            await _userService.DeleteUser(login, revokerLogin, hard);
            return Ok();
        }

        [Authorize("Admin")]
        [HttpPatch("recover/{login}")]
        public async Task<IActionResult> RecoverUser(string login)
        {
            await _userService.RecoverUser(login);
            return Ok();
        }

        [Authorize]
        [HttpPatch("update/{login}/name")]
        public async Task<IActionResult> UpdateName(string login, [Required] string newName)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateName(login, newName, updaterLogin);
            return Ok();
        }

        [Authorize]
        [HttpPatch("update/{login}/gender")]
        public async Task<IActionResult> UpdateGender(string login, [Required] int newGender)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateGender(login, newGender, updaterLogin);
            return Ok();
        }

        [Authorize]
        [HttpPatch("update/{login}/birthday")]
        public async Task<IActionResult> UpdateBirthday(string login, [Required] DateTime newBirthday)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateBirthday(login, newBirthday, updaterLogin);
            return Ok();
        }

        [Authorize]
        [HttpPatch("update/{login}/password")]
        public async Task<IActionResult> UpdatePassword(string login, [Required] string newPassword)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdatePassword(login, newPassword, updaterLogin);
            return Ok();
        }

        [Authorize]
        [HttpPatch("update/{login}/login")]
        public async Task<IActionResult> UpdateLogin(string login, [Required] string newLogin)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateLogin(login, newLogin, updaterLogin);
            return Ok();
        }
    }
}