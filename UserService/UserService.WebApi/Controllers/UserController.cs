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
    }
}