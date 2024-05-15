namespace UserService.Core.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> Login(string login, string password);
    }
}