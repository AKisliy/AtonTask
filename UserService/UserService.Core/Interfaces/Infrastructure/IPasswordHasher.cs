namespace UserService.Core.Interfaces.Infrastructure
{
    public interface IPasswordHasher
    {
        public string Generate(string password);

        public bool Verify(string password, string passwordHash);
    }
}