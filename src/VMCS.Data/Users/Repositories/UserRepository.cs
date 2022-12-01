using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _context;

        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            return new User
            {
                Id = entity.Id,
                Login = entity.Login,
                Username = entity.Username,
                Password = entity.Password,
                Email = entity.Email
            };
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Users.Select(it => 
            new User { 
                Id = it.Id,
                Login = it.Login,
                Username = it.Username,
                Password = it.Password,
                Email = it.Email
            }).ToListAsync(cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            var entity = new UserDbModel
            {
                Id = Guid.NewGuid().ToString(),
                Login = user.Login,
                Username = user.Username,
                Password = user.Password,
                Email = user.Email
            };

            await _context.Users.AddAsync(entity, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            _context.Users.Remove(entity);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var entity = await _context.Users.FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {user.Id} не найден");

            entity.Login = user.Login;
            entity.Username = user.Username;
            entity.Password = user.Password;
            entity.Email = user.Email;    
        }

        public bool ContainsByLogin(string login) => _context
            .Users
            .Any(user => user.Login == login);
    }
}
