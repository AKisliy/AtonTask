using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using UserService.Core.Interfaces.Services;
using UserService.Core.Models;
using UserService.WebApi.Dto.Request;

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

        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserRegisterRequest request)
        {
            var id = await _userService.CreateUser(_mapper.Map<User>(request));
            return Created($"api/user/{id}", new {UserId = id});
        }
    }
}