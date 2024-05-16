using UserService.Core.Models;

namespace UserService.Core.Interfaces
{
    public interface IUserRepository
    {
        public Task<Guid> CreateUser(User user);

        public Task<User> GetUserByLogin(string login);

        public Task<User> GetUserById(Guid id);

        public IEnumerable<User> GetActiveUsers();

        public IEnumerable<User> GetUsersOlderThan(int age);

        public Task<bool> HasUserWithLogin(string login);
    }
}