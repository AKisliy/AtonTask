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

        /// <summary>
        /// Create new user (only for admin)
        /// </summary>
        /// <param name="request">Body with login, password, name and isAdmin fields</param>
        /// <response code="201">User created</response>
        /// <response code="400">Some fields have invalid data or problems with token</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="409">User with this login already exists</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpPost("create")]
        public async Task<IActionResult> CreateUser(UserRegisterRequest request)
        {
            var creator = HttpContext.GetUserLogin();
            var login = await _userService.CreateUser(_mapper.Map<User>(request), creator);
            var createdUser = await _userService.GetUserByLogin(login);
            return Created($"api/user/{login}", _mapper.Map<UserResponse>(createdUser));
        }

        /// <summary>
        /// Get all active users (only for admin)
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint only for admins)</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpGet("all/active")]
        public IActionResult GetActiveUsers()
        {
            var users = _userService.GetActiveUsers();
            return Ok(users.Select(u => _mapper.Map<UserResponse>(u)));
        }

        /// <summary>
        /// Get User by login (only for admin)
        /// </summary>
        /// <param name="login">Login of user to get info</param>
        /// <response code="200">Success</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="404">User with login wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpGet("{login}")]
        public async Task<IActionResult> GetUserByLogin(string login)
        {
            var user = await _userService.GetUserByLogin(login);
            return Ok(_mapper.Map<UserByLoginResponse>(user));
        }

        /// <summary>
        /// Get current user information (only user himself can get it)
        /// </summary>
        /// <response code="200">Success</response>
        /// <response code="401">User can't perform this action (maybe token is expired)</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser()
        {
            var id = HttpContext.GetUserId();
            var user = await _userService.GetUserById(id);
            return Ok(_mapper.Map<UserResponse>(user));
        }

        /// <summary>
        /// Get all users with who are older than provided age (only for admin)
        /// </summary>
        /// <param name="age">The age to compare with</param>
        /// <response code="200">Success</response>
        /// <response code="400">Invalid data was provided</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpGet("all/{age}")]
        public IActionResult GetUsersOlderThan(int age)
        {
            var users = _userService.GetUsersOlderThan(age);
            return Ok(users.Select(u => _mapper.Map<UserResponse>(u)));
        }

        /// <summary>
        /// Delete user with login (only for admin)
        /// </summary>
        /// <param name="login">The login of user to delete</param>
        /// <param name="hard">Is deletion hard? (True if hard, false if soft)</param>,s
        /// <response code="200">Success</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpDelete("{login}")]
        public async Task<IActionResult> DeleteUser(string login, bool hard)
        {
            var revokerLogin = HttpContext.GetUserLogin();
            await _userService.DeleteUser(login, revokerLogin, hard);
            return Ok();
        }

        /// <summary>
        /// Recover user with login (only for admin)
        /// </summary>
        /// <param name="login">The login of user to recover</param>
        /// <response code="200">Success</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize("Admin")]
        [HttpPatch("{login}/recover")]
        public async Task<IActionResult> RecoverUser(string login)
        {
            await _userService.RecoverUser(login);
            return Ok();
        }

        /// <summary>
        /// Update user's name (for admin or user)
        /// </summary>
        /// <param name="login">The login of user, whose name will be changed</param>
        /// <param name="newName">New name of user. Should only contain Russian or Latin letters.</param>
        /// <response code="200">Success</response>
        /// <response code="400">Some data is invalid</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="403">User was revoked and can't perform this action</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpPatch("{login}/update/name")]
        public async Task<IActionResult> UpdateName(string login, [Required] string newName)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateName(login, newName, updaterLogin);
            return Ok();
        }

        /// <summary>
        /// Update user's gender (for admin or user)
        /// </summary>
        /// <param name="login">The login of user, whose gender will be changed</param>
        /// <param name="newGender">New gender of user. Should be: 0 - female, 1 - male, 2 - unknown.</param>
        /// <response code="200">Success</response>
        /// <response code="400">Some data is invalid</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="403">User was revoked and can't perform this action</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpPatch("{login}/update/gender")]
        public async Task<IActionResult> UpdateGender(string login, [Required, Range(0, 2)] int newGender)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateGender(login, newGender, updaterLogin);
            return Ok();
        }

        /// <summary>
        /// Update user's birthday (for admin or user)
        /// </summary>
        /// <param name="login">The login of user, whose birthday will be changed</param>
        /// <param name="newBirthday">New birthday of user.</param>
        /// <response code="200">Success</response>
        /// <response code="400">Some data is invalid</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="403">User was revoked and can't perform this action</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpPatch("{login}/update/birthday")]
        public async Task<IActionResult> UpdateBirthday(string login, [Required] DateTime newBirthday)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateBirthday(login, newBirthday, updaterLogin);
            return Ok();
        }

        /// <summary>
        /// Update user's password (for admin or user)
        /// </summary>
        /// <param name="login">The login of user, whose password will be changed</param>
        /// <param name="newPassword">New password of user. Should only contain numbers and/or latin letters.</param>
        /// <response code="200">Success</response>
        /// <response code="400">Some data is invalid</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="403">User was revoked and can't perform this action</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpPatch("{login}/update/password")]
        public async Task<IActionResult> UpdatePassword(string login, [Required] string newPassword)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdatePassword(login, newPassword, updaterLogin);
            return Ok();
        }

        /// <summary>
        /// Update user's login (for admin or user)
        /// </summary>
        /// <param name="login">The old login of user, whose login will be changed</param>
        /// <param name="newLogin">New login of user. Should only contain numbers and/or latin letters.</param>
        /// <response code="200">Success</response>
        /// <response code="400">Some data is invalid</response>
        /// <response code="401">User can't perform this action (maybe token is expired or this endpoint is only for Admins)</response>
        /// <response code="403">User was revoked and can't perform this action</response>
        /// <response code="404">User wasn't found</response>
        /// <response code="500">Server problems :(</response>
        [Authorize]
        [HttpPatch("{login}/update/login")]
        public async Task<IActionResult> UpdateLogin(string login, [Required] string newLogin)
        {
            var updaterLogin = HttpContext.GetUserLogin();
            await _userService.UpdateLogin(login, newLogin, updaterLogin);
            return Ok();
        }
    }
}