using UserService.Core.Models;

namespace UserService.Core.Interfaces.Services
{
    public interface IUserService
    {
        public Task<string> CreateUser(User user, string creator);

        public IEnumerable<User> GetActiveUsers();

        public Task<User> GetUserByLogin(string login);

        public Task<User> GetUserById(Guid id);

        public IEnumerable<User> GetUsersOlderThan(int age);

        public Task DeleteUser(string login, string revokerLogin, bool hard);

        public Task RecoverUser(string login);

        public Task UpdateName(string login, string newName, string updaterLogin);

        public Task UpdateGender(string login, int newGender, string updaterLogin);

        public Task UpdateBirthday(string login, DateTime newBirthdate, string updaterLogin);

        public Task UpdatePassword(string login, string newPassword, string updaterLogin);

        public Task UpdateLogin(string login, string newLogin, string updaterLogin);
    }
}