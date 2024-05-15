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

        public async Task<Guid> CreateUser(User user)
        {
            var find = await _userRepository.HasUserWithLogin(user.Login);
            if(find)
                throw new ConflictException("User with this email already exists");
            user.Password = _hasher.Generate(user.Password);
            var id =  await _userRepository.CreateUser(user);
            return id;
        }
    }
}