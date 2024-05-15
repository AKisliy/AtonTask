using UserService.Core.Models;

namespace UserService.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<Guid> CreateUser(User user);
    }
}