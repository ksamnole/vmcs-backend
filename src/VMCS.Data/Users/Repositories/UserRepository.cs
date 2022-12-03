using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.Users;
using VMCS.Core.Domains.Users.Repositories;
using VMCS.Data.Contexts;

namespace VMCS.Data.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationContext _applicationContext;

        public UserRepository(ApplicationContext applicationContext)
        {
            _applicationContext = applicationContext;
        }

        public async Task<User> GetById(string id, CancellationToken cancellationToken)
        {
            var entity = await _applicationContext.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken: cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"Пользователь с Id = {id} не найден");

            return entity;
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _applicationContext.Users.ToListAsync(cancellationToken);
        }

        public async Task Create(User user, CancellationToken cancellationToken)
        {
            await _applicationContext.Users.AddAsync(user, cancellationToken);
        }

        public async Task Delete(string id, CancellationToken cancellationToken)
        {
            var entity = await _applicationContext.Users.FirstOrDefaultAsync(it => it.Id == id, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"User with id = {id} not found");

            _applicationContext.Users.Remove(entity);
        }

        public async Task Update(User user, CancellationToken cancellationToken)
        {
            var entity = await _applicationContext.Users.FirstOrDefaultAsync(it => it.Id == user.Id, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"User with id = {user.Id} not found");
            
            entity.Username = user.Username;
            entity.Email = user.Email;    
        }

        public bool ContainsByLogin(string login) => _applicationContext
            .Users
            .Any(user => user.Login == login);
    }
}
