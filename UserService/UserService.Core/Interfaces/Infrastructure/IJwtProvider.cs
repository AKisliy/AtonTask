using UserService.Core.Models;

namespace UserService.Core.Interfaces.Infrastructure
{
    public interface IJwtProvider
    {
        public string GenerateToken(User user);
    }
}