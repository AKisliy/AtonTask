using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Interfaces.Infrastructure;
using UserService.Core.Interfaces.Services;

namespace UserService.Application.Services
{
    public class AuthService(IUserRepository userRepository, IJwtProvider provider, IPasswordHasher hasher): IAuthService
    {
        private readonly IJwtProvider _provider = provider;
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IPasswordHasher _hasher = hasher;

        public async Task<string> Login(string login, string password)
        {
            var user = await _userRepository.GetUserByLogin(login) ?? throw new NotFoundException($"No user with login: {login}");
            if(user.RevokedOn != null)
                throw new ForbiddenException($"User {login} was revoked at {user.RevokedOn}");
            var result = _hasher.Verify(password, user.Password);
            if(!result)
                throw new CredentialsException("Password is incorrect");
            var token = _provider.GenerateToken(user);
            return token;
        }
    }
}