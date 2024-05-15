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
            entity.CreatedOn = DateTime.UtcNow;
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public async Task<User> GetUserByLogin(string login)
        {
            var entity =  await _context.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Login == login);
            return _mapper.Map<User>(entity);
        }

        public async Task<bool> HasUserWithLogin(string login)
        {
            return await _context.Users.AnyAsync(u => u.Login == login);
        }
    }
}