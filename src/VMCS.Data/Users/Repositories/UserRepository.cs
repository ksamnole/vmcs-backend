using System.Data.Entity.Core;
using Microsoft.EntityFrameworkCore;
using VMCS.Core.Domains.ChannelInvitations;
using VMCS.Core.Domains.Channels;
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
                throw new ObjectNotFoundException($"User with Id = {id} not found");

            return entity;
        }

        public async Task<IEnumerable<User>> GetAll(CancellationToken cancellationToken)
        {
            return await _applicationContext.Users.ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<Channel>> GetAllUserChannels(string userId, CancellationToken cancellationToken)
        {
            var entity = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);

            if (entity == null)
                throw new ObjectNotFoundException($"User with Id = {userId} not found");

            await _applicationContext.Entry(entity).Collection(x => x.Channels).LoadAsync(cancellationToken);

            return entity.Channels;
        }

        public async Task<IEnumerable<ChannelInvitation>> GetAllUserChannelInvitations(string userId, CancellationToken cancellationToken)
        {
            var entity = await _applicationContext.Users.FirstOrDefaultAsync(x => x.Id == userId, cancellationToken);
            
            if (entity == null)
                throw new ObjectNotFoundException($"User with Id = {userId} not found");
            
            await _applicationContext.Users.LoadAsync(cancellationToken);
            await _applicationContext.Channels.LoadAsync(cancellationToken);

            return await _applicationContext.ChannelInvitations.Where(x => x.RecipientId == userId)
                .ToListAsync(cancellationToken);
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
