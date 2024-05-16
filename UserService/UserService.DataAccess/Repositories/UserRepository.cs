using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UserService.Core.Exceptions;
using UserService.Core.Interfaces;
using UserService.Core.Models;

namespace UserService.DataAccess.Repositories
{
    public class UserRepository: IUserRepository
    {
        private readonly AtondbContext _context;
        private readonly IMapper _mapper;

        public UserRepository(AtondbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> CreateUser(User user)
        {
            if(await HasUserWithLogin(user.Login))
                throw new ConflictException("User with this login already exists");
            var entity = _mapper.Map<UserEntity>(user);
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var entity =  await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login) 
                            ?? throw new NotFoundException($"No user with login {login}");
            return _mapper.Map<User>(entity);
        }

        public async Task<User> GetUserById(Guid id)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(u => u.Id == id) 
                            ?? throw new NotFoundException($"No user with id: {id}");
            return _mapper.Map<User>(entity);
        }

        public IEnumerable<User> GetActiveUsers()
        {
            var users = _context.Users.AsNoTracking().Where(x => x.RevokedOn == null).OrderBy(u => u.CreatedOn);
            return users.Select(u => _mapper.Map<User>(u));
        }

        public async Task<bool> HasUserWithLogin(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }

        public IEnumerable<User> GetUsersOlderThan(int age)
        {
            DateTime minDate = DateTime.UtcNow.AddYears(-age);
            var users = _context.Users
                                    .AsNoTracking()
                                    .Where(u => u.BirthDate <= minDate)
                                    .Select(u => _mapper.Map<User>(u));
            return users;
        }
    }
}