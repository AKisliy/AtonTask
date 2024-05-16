using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Interfaces.Infrastructure;
using UserService.Core.Interfaces.Services;
using UserService.Core.Models;

namespace UserService.Application.Services
{
    public class UsersService: IUserService
    {
        private IUserRepository _userRepository;
        private IPasswordHasher _hasher;

        public UsersService(IUserRepository userRepository, IPasswordHasher hasher)
        {
            _userRepository = userRepository;
            _hasher = hasher;
        }

        public async Task<Guid> CreateUser(User user, string creator)
        {
            var find = await _userRepository.HasUserWithLogin(user.Login);
            if(find)
                throw new ConflictException("User with this email already exists");
            user.Password = _hasher.Generate(user.Password);
            user.CreatedBy = creator;
            var id =  await _userRepository.CreateUser(user);
            return id;
        }

        public IEnumerable<User> GetActiveUsers()
        {
            return _userRepository.GetActiveUsers();
        }

        public async Task<User> GetUserByLogin(string login)
        {
            if(!await _userRepository.HasUserWithLogin(login))
                throw new NotFoundException($"No user with login {login}");
            return await _userRepository.GetUserByLogin(login);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var user = await _userRepository.GetUserById(id);
            if(user.RevokedOn != null)
                throw new ForbiddenException($"User was revoked at {user.RevokedOn}");
            return user;
        }

        public IEnumerable<User> GetUsersOlderThan(int age)
        {
            if(age < 0)
                throw new BadRequestException("Age can't be negative");
            return _userRepository.GetUsersOlderThan(age);
        }

        public async Task DeleteUser(string login, string revokerLogin, bool hard)
        {
            if(!await _userRepository.HasUserWithLogin(login))
                throw new NotFoundException($"No user with login {login}");
            await _userRepository.DeleteUser(login, revokerLogin, hard);
        }

        public async Task RecoverUser(string login)
        {
            if(!await _userRepository.HasUserWithLogin(login))
                throw new NotFoundException($"No user with login {login}");
            var user = await _userRepository.GetUserByLogin(login);
            user.RevokedOn = null;
            user.RevokeBy = null;
            await _userRepository.Update(user);
        }
    }
}