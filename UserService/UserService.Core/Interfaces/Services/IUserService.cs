using UserService.Core.Models;

namespace UserService.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<Guid> CreateUser(User user, string creator);

        public IEnumerable<User> GetActiveUsers();

        public Task<User> GetUserByLogin(string login);

        public Task<User> GetUserById(Guid id);

        public IEnumerable<User> GetUsersOlderThan(int age);
    }
}